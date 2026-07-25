# Plano de Testes — ConsolePlus + PromptPlus

> Driver de console **headless** para validar **posicionamento** e **estilo** dos dois produtos,
> rodando em **Windows e Linux**. Decisão de isolamento: **B — isolamento total, driver por projeto**.
> Status: **Fase 1 concluída + cobertura extra de `EmacsConsoleBuffer` (2026-07-22)** — piloto Input+Select
> implementado, mais suíte exaustiva de todas as key bindings emacs (`EmacsConsoleBuffer`, usada pela
> maioria dos controles interativos para editar o buffer de resposta). 69 testes verdes
> (59 ConsolePlus + 10 PromptPlus) em net10.0/net9.0/net8.0 no Windows; Linux cobre-se via CI (seção 8,
> ainda não executado). 1 snapshot Verify estabelecido. Próxima: Fase 2 (rollout dos demais controles).

---

## 1. Objetivo e escopo

- Testar, de forma automatizada, determinística e cross-platform, as funcionalidades de **ConsolePlus**
  (camada de terminal) e **PromptPlus** (controles/widgets).
- Validar **onde** cada glifo é escrito (posição) e **com qual estilo** (fg/bg e, quando modelado, atributos).
- Rodar 100% headless em CI (sem terminal real), idêntico em Windows e Linux.

**Fora de escopo (por decisão de ROI):** paridade pixel/glifo com terminais reais, timing de animações,
detecção de capacidade dependente de ambiente real. Ver seção 9.

---

## 2. Restrições de testabilidade descobertas no código (base das decisões)

| Fato | Arquivo | Consequência |
|---|---|---|
| `ConsolePlus` é `static` + `static ctor` (detecta ambiente, cria driver único, troca encoding, inicia `Task` de monitor de tamanho, registra `ProcessExit`/`CancelKeyPress`) | `ConsolePlus.Startup.cs` | Tocar o singleton em teste = estado global; seria serial e sujo. **Evitar.** |
| Saída vai para `Console.Out`/`Console.Error` singletons (`Out => Console.Out`) | `AnsiConsoleAdapter.cs` | Capturar só `Console.Out` não paraleliza. |
| Estilo e cursor são **ANSI inline** (SGR + CSI) quando `SupportsAnsi` | `ConsoleWriter.cs`, `AnsiCommands.cs` | Observáveis se interpretarmos o ANSI. |
| `CursorLeft/Top`, `Width/Height`, `GetCursorPosition()` leem o **`System.Console` real** (defaults em `IOException`) | `EnvironmentUtil.cs`, `AnsiConsoleAdapter.cs` | Capturar `Console.Out` quebra o *read-back* → **inviabiliza testar os controles** que dependem dele. |
| `ReadKey` usa `Console.KeyAvailable`/`Console.ReadKey` (não redirecionável por `SetIn`) | `AnsiConsoleAdapter.cs`, `BaseControlPrompt.WaitKeypress` | Input interativo precisa de **fila de teclas própria**. |
| **Seam de injeção**: `internal sealed PromptPlusControls(IConsole, PromptConfig)`; `BaseControlPrompt<T>(... IConsole console ...)` | `Core/PromptControls.cs`, `Controls/Common/BaseControlPrompt.cs` | Com `InternalsVisibleTo`, dá para instanciar qualquer controle contra um `IConsole` **falso**. |
| PromptPlus já depende de ConsolePlus (`_console = ConsolePlus.Driver`) | `PromptPlus.Startup.cs` | Driver de teste baseado em ConsolePlus **não cria acoplamento novo**. |
| `PromptPlus/src/PromptPlus.csproj:69` referencia ConsolePlus via **`PackageReference` NuGet** (`ConsolePlus.net 0.5.2-Beta`), não via `ProjectReference` para `ConsolePlus/src` | `PromptPlus.csproj` | Um `ProjectReference` de `PromptPlus.Tests` → `PromptPlus/src` traria o ConsolePlus **publicado**, não o build local — o `InternalsVisibleTo` adicionado em `ConsolePlus/src` (seção 6) não alcançaria esse assembly. **Decisão D6, ver seção 12.** |

**Conclusão:** a estratégia viável é um **terminal virtual em memória que implementa `IConsole`/`IConsolePlus`**,
injetado nos controles, reutilizando o writer real do ConsolePlus. Capturar `Console.Out` sozinho **não serve**
para o PromptPlus.

> Identidade atual do projeto: `AssemblyName`/`PackageId` = **`ConsolePlus.net`**, `RootNamespace` = `ConsolePlusLibrary`.
> (Artefatos `obj/**/TerminalPlus.*` são resquício de rename antigo — lixo, ignorar/limpar.)

---

## 3. Estratégia escolhida

**VirtualTerminal**: implementação in-memory de `IConsole` (+ `IConsolePlus`) que **hospeda o `ConsoleWriter`/
`AnsiCommands`/`AnsiColorBuilder` reais** do ConsolePlus. O ANSI emitido pelo código de produção é **interpretado**
por um `TextWriter` que carimba células `(Rune, Style)` num *grid*. O grid é a fonte da verdade para o *read-back*
de cursor/tamanho — mais fiel e determinístico que o adapter real.

Camadas de teste (ordem de valor):
1. **Unidade pura** (ConsolePlus): `Fragment.FromText` (markup), `AnsiColorBuilder` (cor→SGR nas 3 profundidades),
   Emoji, overflow Crop/Ellipsis, `GetDisplayLength` (largura CJK/emoji). Não precisa de terminal.
2. **Render + estado via VirtualTerminal** (ambos): layout inicial, item selecionado, cursor final, máquinas de estado
   (navegação, edição/máscara, validação, filtros, paginação), viewport/ellipsis lateral.
3. **Resize/relayout** (PromptPlus): poucos cenários canônicos via `SizeChanged` + mudança de `Width/Height`.
4. **PTY E2E** (opcional, poucos smokes): ConPTY/pty rodando samples, snapshot da tela — só para fidelidade.

---

## 4. Estrutura de projetos — decisão B (isolamento total)

Cada projeto de teste é **autossuficiente**: contém sua própria cópia do driver, sem `ProjectReference` para
uma lib de testes compartilhada. Nenhum acoplamento de binário entre `ConsolePlus.Tests` e `PromptPlus.Tests`.

```
tests/
  ConsolePlus.Tests/
    Driver/                       # driver isolado desta suíte
      VirtualTerminal.cs
      VirtualScreen.cs
      AnsiScreenInterpreter.cs
      InputQueue.cs
      ScreenAssertions.cs
      VirtualTerminalOptions.cs
    Unit/                         # camada 1 (componentes puros)
    Rendering/                    # camada 2 (via VirtualTerminal)
    ConsolePlus.Tests.csproj      # ProjectReference -> ../../ConsolePlus/src/ConsolePlus.csproj
  PromptPlus.Tests/
    Driver/                       # MESMO driver, cópia isolada
      ...
    Controls/                     # camada 2 por controle
    Resize/                       # camada 3
    PromptPlus.Tests.csproj       # ProjectReference -> ../../PromptPlus/src/PromptPlus.csproj
```

### 4.1 Mecanismo de "cópia" do driver — decisão interna a B

Para honrar "driver por projeto" **sem** manutenção em dobro divergir, o padrão recomendado é
**source linkado a partir de uma fonte única**, compilado dentro de cada suíte (cada assembly de teste
sai self-contained; não há dependência de binário/lib compartilhada — o isolamento de saída de B é mantido):

```
tests/_driver-src/*.cs            # fonte única do driver (não é projeto)
```
```xml
<!-- em cada *.Tests.csproj -->
<ItemGroup>
  <Compile Include="..\_driver-src\**\*.cs" LinkBase="Driver" />
</ItemGroup>
```

- **Recomendado (default):** linked source acima → um só código-fonte, duas compilações isoladas.
- **Alternativa (isolamento estrito no nível de fonte):** copiar fisicamente os arquivos em cada `Driver/`.
  Custo: duplicação real da peça mais complexa; usar só se a exigência for zero compartilhamento inclusive de fonte.

> **Decisão em aberto (D1):** confirmar linked-source (default) vs cópia física. Ver seção 12.

> **Atualização 2026-07-23 — D1 revertida, e a estrutura acima mudou de local.** Depois de fechada,
> D1 (linked-source único em `tests/_driver-src` no topo do repo) foi invalidada: ConsolePlus e
> PromptPlus são dois repositórios GitHub **distintos**, cada um com seu próprio `.git` — só coexistem
> na mesma pasta local por conveniência do desenvolvedor. Um `tests/` no topo (fora de ambos os
> `.git`) não pertence a nenhum dos dois repos publicados: quem clona qualquer um isoladamente não
> recebe pasta de teste nenhuma. D1 foi revertida pra **cópia física** (a alternativa já prevista
> nesta seção): `ConsolePlus/tests/` e `PromptPlus/tests/` agora são as raízes reais, cada uma com sua
> própria cópia de `_driver-src/` e seu próprio projeto de testes — a estrutura ilustrada acima existe,
> mas repetida uma vez dentro de cada repo, não uma vez no topo. Ver
> `docs/testing-driver-maintenance.md` (idêntico nos dois repos) para o procedimento de manter as duas
> cópias em sincronia, e a decisão revista no início da seção 12 (D1).

---

## 5. Componentes do driver (VirtualTerminal)

- **`VirtualScreen`** — `Cell[Height,Width]` com `Cell(Rune Glyph, Style Style)`, cursor (`Left/Top`), `Current` style;
  `Put` (respeita wrap na largura), `EraseToEol`, `EraseDisplay`, `Clear`; leitura `StyleAt/CharAt/TextAt/Snapshot`.
- **`AnsiScreenInterpreter : TextWriter`** — recebe o stream ANSI do writer real e interpreta **apenas o subconjunto
  que o ConsolePlus emite** (ver `IAnsiCommands`): `CUP` (posição), `EL` (erase-in-line), `ED` (erase-display),
  `SGR ...m` (cor 4/8/24-bit → `Style`), show/hide cursor, enter/exit alt-screen. **Falhar explicitamente** em
  sequência desconhecida (evita falso-verde).
- **`VirtualTerminal : IConsole, IConsolePlus`** — `SupportsAnsi => true` (força caminho ANSI determinístico);
  `Width/Height` fixos por opção; `GetCursorPosition` lê do grid (read-back fiel); `SetCursorPosition` via
  `AnsiCommands` real; `Out => interpreter`; hospeda `ConsoleWriter(this)` de produção; delega markup/estilo/overflow.
- **`InputQueue`** — `Enqueue(ConsoleKey/ConsoleKeyInfo/string)`; alimenta `KeyAvailable`/`ReadKey`/`ReadKeyAsync`.
- **`ScreenAssertions`** — verbos: `CharAt`, `TextAt`, `StyleAt`, `RegionSnapshot`, integração snapshot (Verify).
- **`VirtualTerminalOptions`** — `Width`, `Height`, `ColorDepth`, `SupportsUnicode`, `Interactive`.

**Esqueletos de código completos no Apêndice A** (este documento é autocontido: dá para implementar sem o histórico da conversa).

---

## 6. InternalsVisibleTo (mudança de produção mínima)

Nenhuma mudança de comportamento; só atributos:

- **ConsolePlus** concede IVT a: `ConsolePlus.Tests` **e** `PromptPlus.Tests`
  (ambas embedam o driver, que toca `IConsolePlus`, `ConsoleWriter`, `Fragment`, `ProfileConsole`).
- **PromptPlus** concede IVT a: `PromptPlus.Tests`
  (para `PromptPlusControls`, `PromptConfig`, `BaseControlPrompt<T>` e controles internos).

---

## 7. Ferramentas

- **Framework de teste:** xUnit (padrão .NET). Assertions: FluentAssertions.
- **Snapshot:** Verify (golden files do grid completo de um frame — ótimo para "layout inteiro do controle").
- **Build:** referenciar `src/*.csproj` diretamente (o `.slnx` está quebrado — ver memória de build).
- **TargetFrameworks:** espelhar os do `src` (net8.0; net9.0; net10.0).

---

## 8. CI (Windows + Linux)

GitHub Actions, matriz `os: [windows-latest, ubuntu-latest]`:
```
dotnet test tests/ConsolePlus.Tests/ConsolePlus.Tests.csproj
dotnet test tests/PromptPlus.Tests/PromptPlus.Tests.csproj
```
Camadas 1–3 são headless → resultado idêntico nos dois SOs. Camada 4 (PTY), se existir, roda em job separado
não-bloqueante (tolerante a flakiness/divergência de SO).

> **Atualização 2026-07-23.** Caminhos reais pós-migração (seção 4):
> `ConsolePlus/tests/ConsolePlus.Tests/ConsolePlus.Tests.csproj` e
> `PromptPlus/tests/PromptPlus.Tests/PromptPlus.Tests.csproj` (cada um dentro do próprio repo). O
> `.slnx` de cada repo passou a incluir o projeto de testes e hoje builda sem erro (a nota "`.slnx`
> está quebrado" da seção 7 não reflete mais o estado atual — carrega e builda normalmente). Ainda
> assim, `ci.yml`/`publish-nuget.yml` **não** dependem do `.slnx`/auto-discovery: cada um chama
> `dotnet build src/*.csproj --configuration Release` (o que é empacotado) e
> `dotnet test tests/*.Tests/*.Tests.csproj --configuration Debug` (onde o driver tem acesso aos
> internos via `ProjectReference`, decisão D6) como dois passos explícitos e independentes — nunca
> `dotnet build`/`dotnet test` sem projeto/config explícitos. Motivo: com os testes agora dentro do
> repo, um `dotnet build --configuration Release` "cego" (que descobre o `.slnx` inteiro) tentava
> compilar também o projeto de testes em Release — e isso quebrava no PromptPlus, porque em Release
> o `PromptPlus/src` resolve o ConsolePlus via `PackageReference` (pacote publicado, sem
> `InternalsVisibleTo` pro driver), não via `ProjectReference` (D6). Validado localmente rodando os
> comandos exatos do CI nos dois repos antes de confiar neles.

---

## 9. O que testar — faixas de ROI

**ROI alto (fazer):**
- ConsolePlus puro: markup, cor→SGR (3 profundidades), emoji, overflow Crop/Ellipsis, largura CJK/emoji.
- Render inicial de cada controle: prompt, colunas, estilo do item selecionado, posição final do cursor.
- Máquinas de estado: navegação, edição/máscara, validação, filtros do Select, paginação.
- Posicionamento sensível: viewport, ellipsis lateral (`WriteAnswerViewport`, `ViewportSlice`).

**ROI médio (parcimônia):**
- Resize/relayout do `BaseControlPrompt` (parte mais complexa/bugável): cobrir ~4–5 cenários canônicos
  (shrink/grow largura, shrink altura c/ scroll), não a matriz inteira.

**ROI baixo (NÃO automatizar; manual/visual):**
- Fidelidade em terminal real (WT vs VT100 vs conhost legacy; wrapping/scrollback/emoji largo real).
- Timing de animações (testar estado, não o "quando").
- Detecção de capacidades (`EnvironmentUtil`/`AnsiDetector`) — testar as funções puras com env-vars mockadas,
  nunca o singleton.

---

## 10. API de asserção (posição + estilo)

```csharp
vt.CursorLeft.Should().Be(12);
vt.TextAt(row: 2, col: 4, len: 5).Should().Be("Nome:");
vt.StyleAt(row: 2, col: 4).Foreground.Should().Be(Color.Cyan1);
vt.StyleAt(row: 3, col: 0).Should().Be(cfg.SelectedStyle);
vt.Snapshot().Should().MatchVerified();
```

---

## 11. Fases de execução (pilot-then-rollout)

- **Fase 0 — Fundação** ✅ **CONCLUÍDA (2026-07-22)**
  - Criar `tests/ConsolePlus.Tests` e `tests/PromptPlus.Tests` (multi-target, ProjectReference a `src/*.csproj`).
  - Adicionar os `InternalsVisibleTo` (seção 6) — `ConsolePlus/src/Properties/AssemblyInfo.cs`, `PromptPlus/src/Properties/AssemblyInfo.cs`.
  - Corrigir D6 (`PromptPlus/src/PromptPlus.csproj`: `ProjectReference` condicional em Debug para o ConsolePlus local).
  - Implementar o driver (fonte única em `_driver-src`, linkado) — `VirtualScreen`/`Cell` + `AnsiScreenInterpreter` + `VirtualTerminal`(+`.Overloads`) + `InputQueue` + `ScreenAssertions`.
  - **DoD:** um "hello" via `ConsolePlus`/`VirtualTerminal` grava glifos com estilo correto no grid; build/teste verde em net10.0/net9.0/net8.0 no Windows (`dotnet test` nos dois `*.Tests.csproj`). Linux ainda não validado localmente — cobre-se via CI (seção 8/A.10), pendente de execução.
  - **Correções encontradas na implementação (não previstas no Apêndice A original):** `ResetColor()`/`Clear()` precisam setar `ForegroundRgbColor`/`BackgroundRgbColor` (que disparam `ConsoleWriter.ApplyStyle`) em vez de mutar campos privados direto, senão `VirtualScreen.Current` fica dessincronizado do estado real; `IConsolePlus.Writer` exige implementação explícita de interface (`ConsoleWriter IConsolePlus.Writer => _writer;`) porque `ConsoleWriter` é internal e um membro implícito (público) não pode expor um tipo menos acessível (CS0053); `IConsole.Write(string?, Style)`/`WriteLine(string?, Style)` são overloads de 2 argumentos distintas da versão de 3 argumentos com default — precisam de implementação própria, um parâmetro default não as satisfaz.

- **Fase 1 — Piloto** ✅ **CONCLUÍDA (2026-07-22)**
  - ConsolePlus: unidade pura de markup + cor→SGR + overflow (camada 1) — `tests/ConsolePlus.Tests/Unit/*`,
    `Rendering/OverflowTests.cs`. 20 testes.
  - PromptPlus: `Input` e `Select` (render inicial + navegação + estilo do selecionado + cursor) —
    `tests/PromptPlus.Tests/Controls/{SelectControlTests,InputControlTests}.cs`. 8 testes + 1 snapshot.
  - **DoD:** ✅ 30 testes verdes (meta era ~15–25) em net10/9/8 no Windows (Linux via CI, seção 8, ainda
    não executado); API de asserção validada (`TextAt`/`StyleAt`/`GetCursorPosition`/`Find`); 1 snapshot
    Verify estabelecido (`SelectControlSnapshotTests.cs` + `.verified.txt`, idêntico nos 3 TFMs).
  - **3 achados de arquitetura documentados na seção 13** (hang em `WaitKeypress` sem tecla terminal;
    `InputQueue.Enqueue(Enter)` não confirmava fora do Windows, corrigido; `VirtualTerminal` com
    `Width<80`/`Height<10` trava `Run()` para sempre no aviso de terminal pequeno).

- **Extra (pedido do usuário, 2026-07-22): cobertura de `EmacsConsoleBuffer`** — `ConsolePlus/src/Shared/EmacsConsoleBuffer.cs`,
  classe **pública** que sustenta o buffer de edição de resposta (Emacs-style) usado por `Input` e pelo
  viewport de resposta de outros controles. Unidade pura (camada 1, sem `VirtualTerminal` — o método
  central `TryAcceptedReadlineConsoleKey` recebe `ConsoleKeyInfo` direto). 39 testes em
  `tests/ConsolePlus.Tests/Unit/EmacsConsoleBufferTests.cs` cobrindo **todas** as key bindings
  implementadas: Ctrl+A/B/C/D/E/F/H/K/L/T/U/W, Alt+D/F/L/U, teclas físicas (Home/End/setas/Delete/
  Backspace/Insert/Tab), Enter (sempre rejeitado), `enableEmacsKeys=false` (desliga atalhos mas mantém
  teclas físicas), `readonly`, `maxlength` (achado: retorna `true`/aceito mesmo quando o limite bloqueia
  a inserção), `CaseOptions.Uppercase/Lowercase`, função `validate`, `LoadPrintable`, `ToForward`/`ToBackward`.
  Valores exatos de operações não-triviais (`Ctrl+W`/`Ctrl+T`/`Alt+U`/`Alt+L`/`Alt+D`/`Alt+F`/`Alt+B`/`Ctrl+C`)
  foram confirmados rodando uma sonda descartável e observando o resultado real, não por simulação manual —
  `BackwardWord` (Alt+B) tem uma peculiaridade real: só para depois de cruzar **dois** limites de palavra,
  então de "hello world foo" (fim) um único Alt+B pula "foo" inteiro e para no início de "world", não em "foo".

- **Extra (pedido do usuário, 2026-07-22): cobertura de classes fundacionais pequenas do ConsolePlus**
  ("base para as classes mais complexas", buscando bugs escondidos). 6 classes cobertas, 145 testes novos,
  **1 bug real encontrado e corrigido**:
  - `ConsoleKeyInfoExtensions.cs` (53 testes) — todos os `IsPressXKey`/`IsPressXKeyOrEmacs`. Sem bug novo,
    mas o teste de `IsPressEnterKey` cobre a divergência Windows/não-Windows já conhecida via branching em
    runtime (`OperatingSystem.IsWindows()`) em vez de um pacote de skip — o CI Windows+Linux (seção 8)
    exercita os dois ramos naturalmente, cada um no seu runner.
  - `DashUtil.cs` (27 testes) — `GetBorderUp`/`GetBorderDown` para as 10 combinações de `DashOptions` × unicode/ascii. Sem bug.
  - `StyleExtensions.cs` (6 testes) — `Colors`/`ForeGround`/`Background`/`Overflow`. Sem bug.
  - `ColorExtensions.cs` + `Color.cs` (22 testes) — `Weighted`, `Blend`, `GetLuminance`, `GetContrast`,
    `ToHex`/`FromHex`/`TryFromHex`, `FromInt32`, `FromConsoleColor`↔`ToConsoleColor`.
    **🐛 Bug real corrigido**: `Color.GetInvertedColor()` (`Shared/Color.cs:78-81`) comparava
    `GetLuminance() < 140`, mas `GetLuminance()` (fórmula WCAG) sempre retorna um valor em **[0,1]** — a
    condição era sempre verdadeira, então o método **sempre retornava `White`**, para qualquer cor
    (confirmado: `White.GetInvertedColor()` também dava `White`, quando deveria dar `Black`). O método
    irmão `GetContrastForegroundColor` já usava o threshold correto (`0.5`) na mesma escala. Corrigido para
    `GetLuminance() < 0.5`; só usado em samples/docs, nenhum controle de produção dependia do valor errado.
  - `ColorPalette.cs` + `ColorTableCss.cs` (20 testes) — `ExactOrClosest` (match exato + "closest" via
    fórmula redmean) nas 3 profundidades; tabela CSS (`TryGetColor`/`TryGetName`/`TryGetWeightedColor`,
    incluindo nomes com peso tipo `"red500"`). Sem bug.
  - `MarkupColorTokenizer.cs` (17 testes) — parser char-a-char por trás de `Fragment.FromText`: tags
    abertas/fechadas, `[[`/`]]` escapados, tags malformadas (`[/x]`, `[tag` sem fechar, `[tag[next]`),
    posição de token. Sem bug — o parser é mais fault-tolerant do que aparenta (degrada pra texto puro
    consistentemente em vez de lançar).
  - Total após este lote: **204 testes** em `ConsolePlus.Tests` (+ 10 em `PromptPlus.Tests`) verdes em
    net10.0/net9.0/net8.0 no Windows.

- **2º lote de classes fundacionais (pedido do usuário, 2026-07-22, "continua com mais"): 57 testes novos,
  2º bug real encontrado e corrigido**:
  - `Markup.cs` (11 testes) — `Escape`/`Remove`/`Length` sobre o `MarkupColorTokenizer` já testado;
    round-trip `Remove(Escape(x)) == x` confirmado. Sem bug.
  - `ColorJsonConverter.cs` (6 testes) + `CultureInfoJsonConverter.cs` (7 testes) + `CultureExtensions.cs`
    (2 testes) — descobertos ao explorar vizinhos do `Markup.cs`.
    **🐛 2º bug real corrigido**: `ColorJsonConverter.Read` (`Core/ColorJsonConverter.cs:34-41`) só
    capturava `ArgumentException`, mas hex com dígitos inválidos (ex.: `"#GGGGGG"`) faz `Color.FromHex`
    lançar `FormatException` (via `byte.Parse`) — não capturado, vazava cru pro consumidor em vez do
    `JsonException` com mensagem legível que o método promete. Corrigido para
    `catch (Exception ex) when (ex is ArgumentException or FormatException)`.
  - `StringExtensions.NormalizeNewLines`/`SplitLines` (9 testes) — comportamento cross-platform via
    `Environment.NewLine` (não hardcoded `"\r\n"`, pra funcionar em Windows e Linux). Achado não-óbvio
    documentado num teste: `\r` solto (estilo Mac antigo) é apenas removido, não convertido — a quebra de
    linha se perde. Sem bug (comportamento documentado no comentário do código).
  - `AnsiCommands.cs` (22 testes) — formatação literal de CSI/SGR pra **todos** os comandos (não só o
    subconjunto que `AnsiScreenInterpreter` reconhece), usando um `IConsolePlus` fake mínimo (só
    `Out`/`Error`/`WriteToErrorOutput` reais, resto `NotImplementedException`) em vez de `VirtualTerminal`,
    já que comandos como setas/tabulação/scroll/save-restore-cursor não são interpretados pelo driver e
    lançariam `NotSupportedException`. Sem bug de produção — 1 erro no próprio teste (`"\x1bD"` em C# é
    interpretado como um único char Unicode U+01BD, já que `\x` consome até 4 dígitos hex e `D` é hex
    válido; corrigido para `"\x1b" + "D"`).
  - Total após os dois lotes extra: **261 testes** em `ConsolePlus.Tests` (+ 10 em `PromptPlus.Tests`)
    verdes em net10.0/net9.0/net8.0 no Windows.

- **3º lote de classes fundacionais (pedido do usuário, 2026-07-22): 16 testes novos, sem bug**:
  - `Helper.cs` (15 testes) — `MainToken`/`ExitCode`/`LastException` são estado global mutável
    compartilhado por todo o assembly ConsolePlus. Cada teste salva o valor original e restaura em
    `finally`, para não vazar estado pra outra classe de teste rodando em paralelo (xUnit paraleliza
    entre classes por padrão).
  - `TextWriterExtensions.cs` (4 testes) — `IsStandardOut`/`IsStandardError` são comparação de
    referência contra `System.Console.Out`/`Error`; só *leem* o singleton real (nunca redirecionam),
    então não violam a regra do plano de "evitar o singleton" — essa é uma leitura inofensiva.
  - `Emoji.cs` — resolução de shortcode qualificado por grupo (`"activities/balloon"`,
    `Shared/Emoji.cs:51-78,104-157`), aprofundando `EmojiTests.cs` (que só cobria nomes simples):
    grupo válido resolve, case-insensitive, grupo desconhecido rejeita (não cai pra busca de texto
    livre), nome desconhecido dentro de grupo válido retorna vazio, alias de grupo sem "And"
    (`"peoplebody/thumbs_up"` via `PeopleAndBody`) também resolve. 8 testes, sem bug.
  - Total após os três lotes extra: **277 testes** em `ConsolePlus.Tests` (+ 10 em `PromptPlus.Tests`)
    verdes em net10.0/net9.0/net8.0 no Windows.

- **Classes fundacionais/de apoio do PromptPlus (pedido do usuário, 2026-07-22, "mesmos critérios"):
  45 testes novos, 3º e 4º bugs reais encontrados e corrigidos**:
  - `Paginator<T>` (`Controls/Common/Paginator.cs`, 20 testes) — motor de paginação reusado por
    ~10 controles (Select, MultiSelect, Tree, MultiTree, MultiTasks, MultiFile, ChartBar, Table,
    MultiTable, FileExec, Calendar). **🐛 3º bug real corrigido**: `UpdateCollection()` não resetava
    `SelectedPage` antes de chamar `FirstItem()` — recarregar uma coleção **mais curta** enquanto
    parado numa página posterior deixava o paginador sem seleção (`SelectedIndex=-1`) mesmo havendo
    itens válidos na nova coleção. O método irmão `UpdateFilter()` já resetava `SelectedPage=0`
    corretamente; `UpdateCollection()` só esqueceu. **Afeta produção de verdade**: ~9 chamadas em
    `MultiSelectControl` e 3 em `MultiTableControl` (features "mostrar só selecionados"/refresh).
    Corrigido adicionando `SelectedIndex=-1; SelectedPage=0;` no início do método, espelhando
    `UpdateFilter`. Também documentado (sem ser bug): `LastItem()`/`FirstItem()` chamados diretamente
    são relativos à página ATUAL, não à coleção inteira — só funcionam com semântica de
    "primeiro/último absoluto" quando chamados via `End()`/`Home()`, que navegam pra página certa
    primeiro; e `NextItem()`/`PreviousItem()` não dão wrap-around nos extremos absolutos da coleção
    (só `NextPage`/`PreviousPage` dão wrap, via `%PageCount`).
  - `Optional<T>` (`Controls/Common/Optional.cs`, 13 testes). **🐛 4º bug real corrigido (latente)**:
    `operator ==(T left, Optional<T> right)` comparava `left` contra `right.Value` sem checar
    `right.HasValue` — `0 == Optional<int>.Empty()` retornava `True` (deveria ser `False`). Confirmado
    via investigação que nenhum código de produção usa esse operador hoje (só `.HasValue`/`.Value`
    diretamente); risco latente, não bug visível ainda. Corrigido para checar `HasValue` primeiro.
  - `HotKey` (`Shared/Common/HotKey.cs`, 12 testes). **🐛 5º bug real corrigido (latente)**:
    `KeyInfo` fazia `(char)Key` sem qualquer filtro — funcionava certo só por coincidência pra
    A-Z/D0-D9/Escape/Tab/Backspace/Enter (valores do enum `ConsoleKey` batem com códigos ASCII reais
    nesses casos), mas produzia `KeyChar` sem sentido pra qualquer outra tecla (F1→`'p'`, setas,
    Home/End/PageUp/Down, teclas Oem, etc.). Uma correção inicial por faixa numérica (`>32 and <127`)
    não bastou — muitas teclas especiais caem dentro dessa faixa por coincidência (setas 37-40,
    Home/End 35-36, F1-F14 112-125 se sobrepõem à banda imprimível). Corrigido com allowlist explícita
    (Backspace/Tab/Enter/Escape/Spacebar/D0-D9/A-Z), retornando `'\0'` pra tudo fora dela. `KeyInfo`
    não é usado em nenhum lugar da produção hoje (`grep` confirmou zero chamadas) — risco latente.
  - Total: **332 testes** (277 `ConsolePlus.Tests` + 55 `PromptPlus.Tests`) verdes em
    net10.0/net9.0/net8.0 no Windows. **5 bugs reais de produção** encontrados e corrigidos até agora
    nesta frente de testes fundacionais (2 no ConsolePlus, 3 no PromptPlus).
  - `BufferScreen`/`BufferState`/`LineScreen` (`Controls/Common/{BufferScreen,BufferState,LineScreen}.cs`,
    19 testes) — motor de diff de renderização usado por **todo** controle via
    `BaseControlPrompt.RenderBuffer` (`UpdateBufferDiff`, `PhysicalLineCount` pro cálculo de
    reflow no resize). Sem bug — matemática de `PhysicalLineCount` (clipping pela largura de render
    anterior + wrap na largura nova) e as regras de diff (`UpdateBufferDiff` só reporta linhas
    mudadas ou genuinamente novas, nunca remoções — isso é responsabilidade separada do chamador)
    conferem exatamente com o esperado, confirmado por sonda antes de travar as asserções.
  - Total final: **351 testes** (277 `ConsolePlus.Tests` + 74 `PromptPlus.Tests`) verdes em
    net10.0/net9.0/net8.0 no Windows.

- **3º lote de classes de apoio do PromptPlus (pedido do usuário, 2026-07-22, "siga a ordem até o
  final, pulando AnsiDetector/UnicodeDetector"): 79 testes novos, 6º bug real (duplicado) + 1
  fragilidade latente corrigida + 1 mudança de arquitetura (nova dependência)**:
  - `PromptPlus/src/Core/ColorJsonConverter.cs` (5 testes). **🐛 6º bug real corrigido**: cópia exata
    do `ColorJsonConverter` do ConsolePlus, com o **mesmo bug** (`catch(ArgumentException)` não pega
    `FormatException` de `Color.FromHex`) — mas é uma classe compilada num assembly diferente, então
    a correção do ConsolePlus não valia pra essa. `CultureInfoJsonConverter.cs` (PromptPlus) também é
    cópia exata, mas já estava correta (sem teste extra, coberto pela suíte do ConsolePlus).
  - `MaskEditBuffer<T>`/`MaskElement` (`Controls/MaskEdit/*.cs`, 19 testes) — máquina de estado de
    edição de máscara (string/número/moeda/data-hora), base de toda a família `MaskEdit*Control`.
    Fixtures construídas via reflection chamando os métodos `private static`
    `NormalizeStringMask`/`NormalizeNumberMask` reais de `MaskEditControl<T>` (IVT não relaxa
    `private`), garantindo que o layout dos elementos bate exatamente com a produção. Comportamento
    de entrada numérica "shift-left" (dígito novo empurra os anteriores, backspace remove o
    mais-recente-digitado) e navegação (`Tab`/setas/Home/End/Emacs) confirmados por sonda antes de
    travar as asserções. **Fragilidade latente corrigida**: o construtor indexava
    `_charElements[_decimalposition]` sem checar limites — uma máscara numérica sem separador
    decimal deixava `CursorPosition=-1` e lançava `KeyNotFoundException` na primeira tecla.
    Inalcançável hoje (`MaskEditControl<T>.SetNumberFormat` sempre inclui um `.`, mesmo pra inteiros
    com 0 casas decimais), mas adicionada guarda no construtor (`ArgumentException` imediato) para
    não depender desse invariante silenciosamente pra sempre.
  - `ComponentStyles.cs` — acabou sendo só um **enum** (nomes de região de estilo), sem lógica; nada
    a testar (suposição inicial errada de que seria uma classe de dicionário).
  - `HotKeyJsonConverter.cs` (10 testes) — JSON custom (objeto `{key,alt,ctrl,shift}`), único do
    PromptPlus. Sem bug — validação de token/propriedade já bem escrita.
  - `SpinnerBase.cs` (6 testes, via double de teste já que é abstrata) + catálogo `SpinnerBase.Known`
    (1 teste reflexivo cobrindo todas as entradas) — ciclo de frames circular, base de spinners de
    Task/Progress. Sem bug.
  - `FileHistory.cs` (15 testes) — **mudança de arquitetura, pedida explicitamente pelo usuário**:
    a classe escrevia direto em `%USERPROFILE%\PromptPlus.History\` sem nenhum seam de injeção.
    Adicionado `System.IO.Abstractions` (produção) + `System.IO.Abstractions.TestingHelpers` (testes)
    — `FileHistory.FileSystem` agora é um `IFileSystem` estático substituível (`internal`, default
    `new FileSystem()` real), testes usam `MockFileSystem` (puramente em memória, idêntico em
    Windows/Linux) salvo/restaurado via `IDisposable` por teste, mesmo padrão de
    `HelperTests`. `AddHistory` (puro, sem I/O) testado direto. Sem bug — dedup/expiração/ordenação/
    corte por `maxitem` todos corretos.
  - Total após este lote: **407 testes** (277 `ConsolePlus.Tests` + 130 `PromptPlus.Tests`) verdes em
    net10.0/net9.0/net8.0 no Windows. **6 bugs reais de produção** corrigidos até agora (3 no
    ConsolePlus, 3 diretos + 1 duplicado no PromptPlus — GetInvertedColor, ColorJsonConverter(x2),
    Paginator.UpdateCollection, Optional<T>, HotKey.KeyInfo) + 1 fragilidade latente adicional
    (MaskEditBuffer). (Correção de aritmética: o total/contagem de `PromptPlus.Tests` reportado
    antes, 153, estava errado — o valor correto, confirmado por execução, é 130.)

- **4º lote — classes finais do ConsolePlus (pedido do usuário, 2026-07-22, mesma ordem "siga até o
  final, pulando AnsiDetector/UnicodeDetector"): 36 testes novos, 7º bug real corrigido**:
  - `Figlet/FigletFont.cs` (10 testes, `Unit/FigletFontTests.cs`) — camada 1, unidade pura. Parser do
    formato `.flf` (FIGlet font). Fixture mínima construída em memória (`MemoryStream`), sem tocar o
    recurso embutido real nem o disco: header, `CommentLines`, substituição de hardblank,
    concatenação de glyphs por caractere, `ToAsciiArtMarkup` (fragmentos de conteúdo + quebra de
    linha), assinatura inválida (`InvalidDataException`), caminho nulo/vazio
    (`ArgumentNullException`), arquivo inexistente (`FileNotFoundException`, caminho cross-platform
    via `Path.GetTempPath()`), construtor sem parâmetros carregando a fonte `Standard` embutida. Sem
    bug.
  - `Figlet/BannerWidget.cs` (4 testes, `Rendering/BannerWidgetTests.cs`) — camada 2 (usa
    `IConsolePlus`/`VirtualTerminal` via `Show()`). `Show()` escreve o glyph renderizado na grade;
    `Border(DashOptions)` adiciona linha superior/inferior com a largura da linha mais larga
    renderizada; sem borda por padrão só escreve a linha do glyph; texto vazio não escreve nada.
    Reutiliza a mesma fonte `.flf` mínima de `FigletFontTests`. **Erro de teste autocorrigido antes
    de rodar**: a primeira versão comparava `vt.TextAt(...)` com `string.Empty`, mas células não
    tocadas da grade renderizam como espaço (`' '`), nunca como string vazia — corrigido para
    `.Trim().Should().BeEmpty()`, mesmo padrão de lição já registrada pra `VirtualScreen.Snapshot()`.
    Sem bug.
  - `ConsoleAbstractions/ProfileExtensions.cs` (22 testes, `Unit/ProfileExtensionsTests.cs`) —
    `EnrichersCI()` despacha pra 14 detectores de CI (`RuntimeEnvironment/*.cs`: AppVeyor,
    AzurePipelines, Bamboo, Bitbucket, Bitrise, Continua, GitHub, GitLab, GoCD, Jenkins, MyGet,
    TeamCity, Tfs, Travis), o primeiro que der match nas env vars conhecidas "ganha" e para o loop.
    **Bloqueio de testabilidade confirmado por sonda empírica**: `BaseClassCI` cacheia
    `Environment.GetEnvironmentVariables()` num campo `private static` compartilhado por TODAS as
    subclasses, populado uma vez e nunca invalidado — a sonda provou que, depois do primeiro match
    (ex. GitHub), trocar a env var real e chamar outro detector (GitLab) ainda retornava o resultado
    do primeiro (GitHub), por causa do cache obsoleto. Resolvido via reflection nos testes
    (`BaseClassCI._environmentVariables` zerado antes de cada cenário — mesmo padrão já usado pra
    métodos `private static`), sem alterar código de produção. Cada teste também limpa as 14 env vars
    conhecidas antes/depois (`IDisposable`) pra não depender do ambiente real onde a suíte rodar.
    **🐛 7º bug real corrigido**: `Continua.cs` era o único dos 14 detectores que não setava
    `profile.ChangedColorDepth = true` ao forçar `ColorDepth = FourBit`. Como
    `EnvironmentUtil.cs:315` faz `if (!profile.ChangedColorDepth) { profile.ColorDepth = colordetect; }`
    logo depois de `EnrichersCI()`, o `FourBit` forçado pelo Continua era silenciosamente sobrescrito
    pela auto-detecção — a detecção de CI "funcionava" (retornava `true`, `Interactive=false`) mas o
    `ColorDepth` correto nunca chegava a valer. Corrigido para setar `ChangedColorDepth = true` como
    os outros 13, com teste de regressão dedicado.
  - Total após este lote: **443 testes** (313 `ConsolePlus.Tests` + 130 `PromptPlus.Tests`) verdes em
    net10.0/net9.0/net8.0 no Windows. **7 bugs reais de produção** corrigidos até agora (3 no
    ConsolePlus — GetInvertedColor, ColorJsonConverter, Continua.ChangedColorDepth; 4 no PromptPlus —
    ColorJsonConverter (duplicado do bug do ConsolePlus), Paginator.UpdateCollection, Optional<T>,
    HotKey.KeyInfo) + 1 fragilidade latente adicional (MaskEditBuffer). Lista aprovada pelo usuário
    ("siga a ordem até o final, pulando AnsiDetector/UnicodeDetector") esgotada.

- **Fase 2 — Rollout**
  - Cobrir os demais controles/widgets em ondas, reusando o padrão do piloto (catalog-then-fix).
  - **DoD:** todos os controles com pelo menos render inicial + fluxo principal.

- **Fase 3 — Resize (driver estendido em 2026-07-24)** — continuação da Fase 2, cujo tracker
  detalhado (`FASE2-CONTROLS-PLAN.md`) está congelado em 2026-07-23; os achados abaixo (pós-Fase 2)
  não estão lá, só aqui.
  - `VirtualTerminal.RaiseResize` antes só disparava o evento `SizeChanged`, sem redimensionar o
    `VirtualScreen` de fato (`Width`/`Height` eram `{ get; }`, sem setter) — o subsistema de resize
    da `BaseControlPrompt` era literalmente intestável. Estendido: `VirtualScreen.Resize(w,h)` real
    (realoca a grade preservando a região de interseção nas mesmas coordenadas — sem reflow de
    texto, igual a um terminal real — e blanks nas células recém-expostas; cursor é clampado se
    caiu fora); `RaiseResize` chama esse resize ANTES de disparar o evento.
  - Primeiro teste real de resize (`PromptPlus/tests/PromptPlus.Tests/Controls/
    ResizeRelayoutTests.cs`) achou um **bug real de produção só visível com resize de verdade**:
    `InputControl.TryResult` setava `_updatePosAnswerBuffer = true` antes de ler a próxima tecla e
    só resetava pra `false` no caminho de tecla normal — um `break` por `press.IsResize` saía do
    loop com a flag ainda `true`, e a passagem de render seguinte (o relayout de resize) recarregava
    `_inputdata` a partir de `_lastinput` + `ToHome()`, resetando o cursor de edição pro início do
    texto mesmo sem o usuário ter feito nada. Corrigido movendo o reset pra antes do `if
    (press.IsResize || press.IsCancelled)`.
  - **Achado não perseguido nesta sessão**: o mesmo padrão (`press.IsResize || press.IsCancelled`
    seguido de `break`) existe em outros 18 controles — candidato natural pra uma auditoria futura
    agora que resize é testável, mas fora do escopo desta rodada.

- **Fase 4 — PTY E2E (opcional/baixo ROI)**
  - Poucos smokes rodando samples sob ConPTY/pty.

- **Fase 5 — Documentação do produto (atualização dos docs) ✅ CONCLUÍDA (2026-07-24)** —
  atualização da documentação em `PromptPlus/docs` com base nas mudanças de API públicas descobertas
  na auditoria dirigida por testes (Fases 2/3). A pasta `docs/api` é **intocável** (gerada por
  ferramenta) e ficou fora do escopo. Execução em Inglês, na ordem D1→D5.
  - **Renomeações de métodos públicos documentadas** (verificadas contra o código-fonte real em
    `PromptPlus/src`, não presumidas):
    - `MultiSelect` / `MultiTable` / `MultiTree` / `MultiFile`: `PredicateSelected*` →
      `PredicateChecked*`. Os single-select (`Select`, `Table`, `Tree`, `Calendar`, `ChartBar`,
      `MaskEdit`) **mantêm** `PredicateSelected*`.
    - `Input` / `Secret`: `PredicateSelected*` → `PredicateValid*`.
    - `Slider`: método `Fill(SliderBarType)` → `BarType(SliderBarType)` (o membro do enum
      `SliderBarType.Fill` permanece inalterado).
  - **APIs novas documentadas:**
    - `MultiTree`: parâmetros `disable` / `check` em `Root`/`AddLast`/`AddFirst`/`AddAfter`/`AddBefore`,
      novo tipo de retorno `IMultiTreeNode<T>` e semântica de nós desabilitados (bloqueio de check
      interativo, cascade que atravessa sem marcar, force-check via `Default`, sobrevivência ao
      clear-all).
    - `ChartBar`: geração de `Id` sequencial automático no `AddItem`; `ChartBarOrder.None` é no-op
      (preserva a ordem de inserção).
    - `ProgressBar`: o frame final é sempre renderizado, então `ElapsedTime` reflete o estado real de
      término e coincide com `StateProgress.ElapsedTime`.
  - **Docs de migração (`docs/migration/controls/*.md`):** adicionada uma seção dedicada **"Renamed
    public methods"** em cada controle afetado (`select.md`, `input.md`, `tree.md`, `table.md`,
    `file.md`, `nochanges.md`) e corrigidas as tabelas v5→v6 que ainda listavam nomes antigos como
    "inalterados".
  - **Revisão de consistência:** zero referências obsoletas a `PredicateSelected` nos docs dos
    controles Multi*; as ocorrências remanescentes são legítimas (lado v5 das renomeações ou
    controles que realmente mantêm o nome).
  - Verificação do typo `seleted`→`selected`: **no-op** (grep sem ocorrências nos docs).

---

## 12. Decisões (fechadas em 2026-07-22; D1 revista em 2026-07-23 — ver nota)

- **D1 (mecanismo do driver em B):** ✅ **linked-source único** (`tests/_driver-src/*.cs`, `<Compile Include LinkBase>` em cada `*.Tests.csproj`). Confirmado: nenhum projeto de teste ou padrão de link-source existe hoje no repo (terreno limpo, sem conflito).
  → **REVISTA em 2026-07-23**: linked-source presumia um único repo; como ConsolePlus/PromptPlus são
  dois repos GitHub distintos, D1 foi trocada pra **cópia física** (`ConsolePlus/tests/_driver-src` e
  `PromptPlus/tests/_driver-src`, cópias independentes, cada uma versionada no próprio repo). Ver
  nota na seção 4 e `docs/testing-driver-maintenance.md`. Alternativas rejeitadas na hora (pacote
  NuGet próprio, drivers independentes do zero) documentadas no mesmo arquivo.
- **D2:** ✅ **xUnit + Verify** (não TUnit) — mantém os esqueletos do Apêndice A como estão.
- **D3:** ✅ Confirmado por leitura direta: `ConsolePlus/src/ConsolePlus.csproj:4` e `PromptPlus/src/PromptPlus.csproj:3` usam ambos `net10.0;net9.0;net8.0`. Espelhar exatamente essa lista/ordem nos `*.Tests.csproj`.
- **D4:** ✅ **Só cor (fg/bg), sem atributos.** Verificado em código: `Style` (`ConsolePlus/src/Shared/Style.cs:23-38`) só tem `Foreground`/`Background`/`OverflowStrategy`; `ConsoleWriter.ApplyStyle` (`ConsoleAbstractions/ConsoleWriter.cs:257-268`) só emite SGR de `AnsiColorBuilder` (cor). Produção não emite bold/underline/etc hoje — nada a modelar no grid. Os comentários `// TODO Fase 2: 4-bit / 256` e a nota de D4 no Apêndice A.3/A.5 ficam confirmados como "não aplicável", não como pendência.
- **D5:** ✅ Piloto = **`Input` + `Select`**, confirmado, **com correção de API** em relação ao Apêndice A.9 (ver nota abaixo): `PromptPlusControls.Input(...)`/`.Select<T>(...)` retornam interfaces **públicas** (`IInputControl`, `ISelectControl<T>` — `PromptPlus/src/Core/PromptControls.cs:167-188`), não é preciso `dynamic`/cast para classe concreta interna. Execução é `Run(CancellationToken token = default)` (não `Run()` sem argumento) — `IInputControl.cs:173`, `ISelectControl.cs:37` — retornando `ResultPrompt<T>` (`Shared/Common/ResultPrompt.cs:16-38`) cujas propriedades são **`Content`** (não `Value`) e **`IsAborted`**.
- **D6 (nova, descoberta na verificação — divergência de arquitetura):** `PromptPlus/src/PromptPlus.csproj:69` traz o ConsolePlus via `PackageReference` (NuGet publicado), não `ProjectReference`. Isso quebraria o `InternalsVisibleTo` planejado (seção 6) para `PromptPlus.Tests`, pois o assembly resolvido seria o pacote publicado, não o build local de `ConsolePlus/src`.
  → **Decisão:** `ProjectReference` condicional por `Configuration` em `PromptPlus/src/PromptPlus.csproj` — `PackageReference` em Release (empacotamento normal), `ProjectReference` para `../../ConsolePlus/src/ConsolePlus.csproj` em Debug (testes/dev). Este `.csproj` **já usa esse padrão** (`ItemGroup Condition="'$(Configuration)' == 'Release' ..."` na linha 62 para `DefaultDocumentation`), então a mudança segue um padrão já existente no arquivo, não introduz um novo. Ação concreta na Fase 0 (não aplicada ainda):
    ```xml
    <ItemGroup Condition="'$(Configuration)' != 'Debug'">
      <PackageReference Include="ConsolePlus.net" Version="0.5.2-Beta" />
    </ItemGroup>
    <ItemGroup Condition="'$(Configuration)' == 'Debug'">
      <ProjectReference Include="..\..\ConsolePlus\src\ConsolePlus.csproj" />
    </ItemGroup>
    ```
    `dotnet test` roda em `Debug` por padrão, então `PromptPlus.Tests` passa a puxar o ConsolePlus local automaticamente sem precisar de `-c` explícito.

---

## 13. Riscos e mitigações

- **"Testar o espelho"** (driver e lib compartilharem lógica de largura): o interpretador ANSI é independente do writer;
  largura/overflow são cobertos na camada 1 (unidade pura) contra valores fixos, não contra o próprio driver.
- **Escopo do interpretador:** implementar só o subconjunto emitido; falhar em sequência desconhecida.
- **Divergência do driver duplicado (se D1 = cópia física):** mitigar com linked-source.
- **InternalsVisibleTo:** acopla testes a internos — aceitável em libs deste tipo.
- **Trava (hang) em testes de `Input`/`Select` (achado na Fase 1, 2026-07-22):** `WaitKeypress`
  (`PromptPlus/src/Controls/Common/BaseControlPrompt.cs:745-758`) faz spin-wait em `console.KeyAvailable`
  até haver tecla ou `token` cancelar — **não lança excessão nem retorna se a fila esvaziar antes de uma
  tecla terminal**. Regra obrigatória para todo teste de controle: (1) sempre enfileirar uma tecla terminal
  (Enter para confirmar, Escape para abortar) como última tecla da sequência; (2) sempre passar um
  `CancellationToken` com timeout curto (ex.: 2s) em `Run(token)` como rede de segurança.
- **`InputQueue.Enqueue(ConsoleKey.Enter)` não confirmava nada fora do Windows (achado e corrigido na
  Fase 1, 2026-07-22):** `IsPressEnterKey` (`ConsolePlus/src/Shared/ConsoleKeyInfoExtensions.cs:42-48`)
  compara `.Key` no Windows mas **`.KeyChar` (13/10)** nos demais SOs. O driver original da Fase 0 criava
  `ConsoleKeyInfo` com `KeyChar='\0'` para qualquer `ConsoleKey`, quebrando a promessa "idêntico em
  Windows e Linux" especificamente para Enter. Corrigido em `tests/_driver-src/InputQueue.cs`
  (`DefaultCharFor`): `Enqueue(ConsoleKey.Enter/Tab/Backspace/Escape)` agora usa o `KeyChar` real que um
  console de verdade reportaria para essas teclas. Navegação (setas, Home/End) não é afetada — essas
  comparam só `.Key`.
- **`VirtualTerminal` com `Width`/`Height` abaixo do mínimo trava `Run()` para sempre (achado na Fase 1,
  investigado com o usuário, 2026-07-22):** `RenderBuffer` (`BaseControlPrompt.cs:1523-1530`) tem um
  "small-terminal safeguard": se `console.Height < MinSafeRenderHeight` (10) ou
  `console.Width < MinSafeRenderWidth` (80) — `BaseControlPrompt.cs:67,73` —, chama
  `ShowResizeWarningAndWait`, que bloqueia num loop até o terminal crescer, usando `console.CancelToken`
  (não o `CancellationToken` passado a `Run()`!) como única saída (`BaseControlPrompt.cs:1626-1627`).
  Como `VirtualTerminal.CancelToken => CancellationToken.None` (nunca cancela) e `Width`/`Height` do
  `VirtualScreen` são fixos por design (grid determinístico, sem resize real), esse loop nunca termina —
  nem o `CancellationToken` passado ao teste em `Run(token)` ajuda, porque o safeguard não olha para ele.
  Diagnosticado instrumentando temporariamente `BaseControlPrompt.Run`/`SelectControl.TryResult` com
  breadcrumbs em arquivo (revertido depois, nenhum vestígio ficou em produção). **Não é bug de
  produção** (o safeguard faz sentido para um terminal real onde um humano redimensiona a janela ou
  aperta Ctrl+C) — é uma regra obrigatória para qualquer teste de controle interativo: **sempre usar
  `Width >= 80` e `Height >= 10`** em `VirtualTerminalOptions` (os defaults, seção A.1, já satisfazem
  isso — o problema só aparece se um teste reduzir explicitamente as dimensões). Testar o aviso de
  terminal pequeno em si (ROI baixo, fora do piloto) exigiria plumbing adicional no driver (ex.: linkar
  `VirtualTerminal.CancelToken` a um `CancellationTokenSource` que o teste controla).

---

## 14. Fase 2 (piloto) — cobertura por modo em Select/Input, achados e bug corrigido (2026-07-23)

Depois da migração da seção 4 (D1 revista), o usuário pediu uma avaliação técnica+P.O. de expandir a
cobertura de `Input`/`Select` além do caminho feliz: cada controle tem um `TryResult` com uma cadeia
longa de `if/else if` por tecla, e as classes de apoio (`EmacsConsoleBuffer`, `Paginator` etc.) já
estavam exaustivamente testadas isoladamente — o gap real estava no *roteamento* da tecla pelo
controle + no *render* resultante (VT), camada que os testes de classe de apoio não alcançam.

**Levantamento do código real revelou mais modos do que a suposição inicial:** `Select` tem 2
(`Select`, `Filter`); `Input` tem **3** (`Input`, `History`, `Sugestions` — este último com 2
sub-comportamentos: autocomplete inline vs dropdown). Reorganizado em uma suíte por modo, por
sugestão do usuário:

```
tests/PromptPlus.Tests/Controls/
  SelectControlTests.cs                 # globais + modo Select
  SelectControlFilterModeTests.cs       # modo Filter
  InputControlTests.cs                  # globais + modo Input (edição básica, tooltip)
  InputControlHistoryModeTests.cs       # modo History
  InputControlSuggestionsModeTests.cs   # modo Sugestions (inline + dropdown)
  InputSecretControlTests.cs            # Secret/senha (F2) — API pública distinta (IInputSecretControl)
```

**Descobertas reais de comportamento** (confirmadas por código + execução, não são bug de teste):
- `Paginator.NextPage`/`PreviousPage` usam módulo (`Paginator.cs:220,239`) — paginação é cíclica, não
  "clampada": PageDown na última página volta pra primeira; PageUp na primeira pula pra última.
- `PageUp` usa `IndexOption.LastItemWhenHasPages` — pousa no **último** item da página anterior, não
  no primeiro (assimetria deliberada com PageDown, imita "rolar pra cima").
- `FileHistory.LoadHistory` reordena por `TimeOutTicks` decrescente (`FileHistory.cs:55-57`) — não
  preserva ordem de inserção; ao popular histórico em teste, timeouts iguais/próximos podem inverter
  a ordem esperada por pura corrida de ticks (usar timeouts explicitamente distintos).
- `InputControl` reverte `ModeView.History`/`Sugestions` de volta pra `Input` (restaurando o texto
  anterior) sempre que `Run()` termina por cancelamento fora do modo `Input` — diferente do `Select`,
  cujo modo `Filter` sobrevive ao cancelamento. A técnica "sem tecla, espera cancelar" (funciona no
  Select) não observa esses modos no Input; só um Enter/Tab real revela o estado.
- Abrir o dropdown de sugestões **não** carrega a sugestão destacada no buffer de edição — só
  Tab/Enter (aceitar) fazem isso (`InputControl.cs:459,717`). Editar enquanto só navega o dropdown
  continua a partir do texto de ANTES de abrir, não da sugestão em destaque.
- Shift+Tab só cicla pra trás dentro do dropdown (`autocomplete:false`) — com autocomplete inline
  (`true`, default), Shift+Tab cai no handler genérico e não faz nada.

**Bug real #8 (achado e corrigido)**: `InputControl.TryResult` (`InputControl.cs:415-428`) não setava
`ResultCtrl` no ramo de cancelamento, diferente do `SelectControl` (que seta). Isso fazia `TryResult`
retornar `false` mesmo cancelado; em `BaseControlPrompt.Run` (`BaseControlPrompt.cs:568`), a condição
do `do...while` avalia `TryResult` como parte de si mesma, então um retorno `false` faz o loop rodar
**uma passada extra de render** — essa passada reconstrói o template com `_errorMessage` já limpo (o
`WriteError` já tinha escrito+limpado na passada anterior), apagando da tela qualquer erro de
validação (`PredicateSelected`/`PredicateSelectedAsync` rejeitando) que estivesse visível bem antes do
controle sair de verdade. Confirmado com uma sonda que tira snapshot em pleno voo (thread de fundo,
antes de cancelar) vs. depois do cancelamento: o meio-de-voo mostrava a mensagem certinha, o final
não. Escopo real: só afeta cancelamento externo pegando o controle com um erro na tela — teclas
reais subsequentes (Escape, corrigir o texto) já seguiam o fluxo normal e funcionavam. **Corrigido**
adicionando `ResultCtrl = new ResultPrompt<string>(_inputdata!.ToString(), true);` no ramo de
cancelamento, espelhando o `SelectControl`. Teste de regressão dedicado:
`InputControlTests.Cancellation_while_a_validation_error_is_showing_does_not_erase_it`.

**Achado colateral (infraestrutura de teste, não é bug de produção)**: `InputControlHistoryModeTests`
e `Unit/FileHistoryTests.cs` trocam o mesmo campo estático `FileHistory.FileSystem` por um
`MockFileSystem`; como o xUnit roda classes de teste diferentes em paralelo por padrão, isso causava
corrida intermitente entre as duas classes. Corrigido com `[CollectionDefinition(...,
DisableParallelization = true)]` + `[Collection(...)]` nas duas classes (`FileHistoryCollection`,
definida em `InputControlHistoryModeTests.cs`), forçando-as a rodar sequencialmente uma em relação à
outra — outras classes continuam paralelas normalmente.

**Cobertura adicional pedida explicitamente pelo usuário nesta rodada**: tecla de toggle de tooltip
(F1, cicla o índice) e show/hide (Ctrl+F1) em `Select` e `Input`; suíte separada para o controle
Secret/senha (`InputSecretControlTests.cs` — mascaramento com caractere default `#`/customizado,
F2 pra revelar/ocultar, `MaskSecret(enabledView:false)` desativando o F2, confirmação com o valor real
não-mascarado).

**Correção 2026-07-23 (mesma sessão) — o gap do Ctrl+A/Ctrl+K era coberto, era só uma leitura errada
do código**: a primeira versão desta seção afirmava que `ConsoleHandler.EnabledEmacs`
(`InputControl.cs:357`) lia um singleton estático global (`ConsolePlusLibrary.ConsolePlus.EnabledEmacs`),
e por isso pulava testar Ctrl+A/Ctrl+K através do controle (risco de corrida entre classes de teste,
mesma categoria do `FileHistory.FileSystem`). Isso estava **errado**: `ConsoleHandler` dentro de
`InputControl` (e de todo `BaseControlPrompt<T>`) é a propriedade de **instância**
`BaseControlPrompt.ConsoleHandler => console` (`BaseControlPrompt.cs:149`), não uma classe estática —
então `ConsoleHandler.EnabledEmacs` resolve pra `console.EnabledEmacs`, a propriedade da PRÓPRIA
`VirtualTerminal` injetada (`VirtualTerminal.cs:90`, autoproperty comum, default `false`). Nenhum
estado global envolvido, nenhum risco de corrida. Corrigido: `InputControlTests.cs` agora tem 3
testes (`CtrlA_then_CtrlK_through_the_control_jumps_home_and_kills_to_the_end`,
`CtrlE_through_the_control_jumps_to_the_end`,
`CtrlA_then_CtrlK_is_a_no_op_when_emacs_bindings_are_disabled_on_the_console`), bastando
`vt.EnabledEmacs = true;` na instância antes de `Run()`.

**Lacunas conhecidas, deixadas de fora deliberadamente (não é esquecimento)**:
- `AnsiDetector.cs`/`UnicodeDetector.cs` — **decisão definitiva do usuário (2026-07-23): não serão
  cobertos.** Não é mais uma pendência aberta, é escopo fechado.
- Fase 2 **real** (rollout do mesmo padrão pros demais controles — 18 controles, 4 deles "Live") —
  **planejamento controle a controle criado em 2026-07-23**, ver `tests/FASE2-CONTROLS-PLAN.md`
  (só existe no PromptPlus, já que todos os controles são dele; é o tracker de progresso vivo desta
  frente, atualizar lá — não aqui). Progresso até agora: **Grupo 1 concluído** (KeyPress, Switch,
  Slider, ChartBar — 2 bugs reais de produção corrigidos no ChartBar + 1 gap do driver de teste
  corrigido, SGR de 256 cores nunca implementado); Grupo 5 com infraestrutura pronta (`IFileSystem`
  migrado em `FileControl`/`MultiFileControl`); Grupos 2-4 e 6 não iniciados.

**Total ao fim desta frente**: 313 (ConsolePlus.Tests) + 250 (PromptPlus.Tests) = **563 testes**,
verdes de forma estável em múltiplas execuções completas seguidas (3 TFMs cada). 10 bugs reais de
produção corrigidos no total desde o início do projeto de testes.

---

## Apêndice A — Código do driver (esqueletos completos)

> **Convenções.** Namespace `ConsolePlusLibrary.Testing`. Código em inglês (segue o repo). Os esqueletos
> compilam contra a API real verificada (`IConsole`, `IConsolePlus`, `ConsoleWriter`, `Style`, `Color`,
> `ProfileConsole`, `IAnsiCommands`, `ColorSystem`, `AutoDetect`). Pontos que dependem de confirmação de
> assinatura estão marcados com `// VERIFY`. Fatos de emissão ANSI confirmados no código de produção:
> CSI = `ESC[`; DEC-private = `ESC[?`; CUP = `ESC[{row+1};{col+1}H` (1-indexed); EL = `ESC[{mode}K`;
> ED = `ESC[{mode}J`; SGR termina em `m`; cursor `ESC[?25h`/`ESC[?25l`; alt-screen `ESC[?1049h`/`l`;
> SGR truecolor = `38;2;r;g;b` (fg) / `48;2;r;g;b` (bg); reset = `0`.

### A.1 `VirtualTerminalOptions`

```csharp
namespace ConsolePlusLibrary.Testing;

public sealed class VirtualTerminalOptions
{
    public int Width { get; set; } = 80;
    public int Height { get; set; } = 24;
    public ColorSystem ColorDepth { get; set; } = ColorSystem.TrueColor; // determinístico p/ estilo
    public bool SupportsUnicode { get; set; } = true;
    public bool Interactive { get; set; } = true;
    public Color DefaultForeground { get; set; } = new(192, 192, 192); // VERIFY: cor default desejada
    public Color DefaultBackground { get; set; } = new(0, 0, 0);
}
```

### A.2 `Cell` + `VirtualScreen`

```csharp
using System.Text;

namespace ConsolePlusLibrary.Testing;

public readonly record struct Cell(Rune Glyph, Style Style)
{
    public static Cell Blank(Style style) => new(new Rune(' '), style);
}

public sealed class VirtualScreen
{
    private readonly Cell[,] _cells;      // [row, col]
    public int Width { get; }
    public int Height { get; }
    public int CursorLeft { get; private set; }
    public int CursorTop { get; private set; }
    public Style Current { get; set; }

    public VirtualScreen(int width, int height, Style initial)
    {
        Width = width; Height = height; Current = initial;
        _cells = new Cell[height, width];
        Fill(initial);
    }

    public void Fill(Style style)
    {
        for (int r = 0; r < Height; r++)
            for (int c = 0; c < Width; c++)
                _cells[r, c] = Cell.Blank(style);
    }

    public void SetCursor(int left, int top)
    {
        CursorLeft = Math.Clamp(left, 0, Width - 1);
        CursorTop  = Math.Clamp(top,  0, Height - 1);
    }

    /// <summary>Stamps one rune at the cursor with <see cref="Current"/> and advances (wrap + scroll).</summary>
    public void Put(Rune g)
    {
        if (CursorTop >= Height) ScrollUp(1);
        _cells[CursorTop, CursorLeft] = new Cell(g, Current);
        // NOTE: largura de glifo tratada como 1 aqui; para wide-char (CJK/emoji) avançar 2 e marcar
        // a 2ª célula como continuação. Cobrir na Fase 1 se o piloto exercitar wide-chars. // VERIFY
        if (++CursorLeft >= Width) { CursorLeft = 0; if (++CursorTop >= Height) ScrollUp(1); }
    }

    public void NewLine() { CursorLeft = 0; if (++CursorTop >= Height) { CursorTop = Height - 1; ScrollUp(1); } }
    public void CarriageReturn() => CursorLeft = 0;

    public void EraseInLine(int mode)   // 0=cursor->eol, 1=bol->cursor, 2=linha toda
    {
        int from = mode == 0 ? CursorLeft : 0;
        int to   = mode == 1 ? CursorLeft : Width - 1;
        for (int c = from; c <= to && c < Width; c++) _cells[CursorTop, c] = Cell.Blank(Current);
    }

    public void EraseInDisplay(int mode) // 0=cursor->fim, 1=início->cursor, 2/3=tudo
    {
        if (mode >= 2) { Fill(Current); return; }
        // 0 e 1: limpa a linha do cursor + linhas abaixo/acima
        EraseInLine(mode);
        if (mode == 0) for (int r = CursorTop + 1; r < Height; r++) BlankRow(r);
        else            for (int r = 0; r < CursorTop; r++)          BlankRow(r);
    }

    private void BlankRow(int r) { for (int c = 0; c < Width; c++) _cells[r, c] = Cell.Blank(Current); }

    private void ScrollUp(int n)
    {
        for (int i = 0; i < n; i++)
        {
            for (int r = 1; r < Height; r++)
                for (int c = 0; c < Width; c++) _cells[r - 1, c] = _cells[r, c];
            BlankRow(Height - 1);
        }
        CursorTop = Math.Min(CursorTop, Height - 1);
    }

    // ---- leitura para asserções ----
    public Style StyleAt(int row, int col) => _cells[row, col].Style;
    public Rune  GlyphAt(int row, int col) => _cells[row, col].Glyph;

    public string TextAt(int row, int col, int len)
    {
        var sb = new StringBuilder();
        for (int c = col; c < col + len && c < Width; c++) sb.Append(_cells[row, c].Glyph.ToString());
        return sb.ToString();
    }

    public string Snapshot()               // grid inteiro como texto (trim trailing spaces por linha)
    {
        var sb = new StringBuilder();
        for (int r = 0; r < Height; r++)
        {
            var line = new StringBuilder();
            for (int c = 0; c < Width; c++) line.Append(_cells[r, c].Glyph.ToString());
            sb.AppendLine(line.ToString().TrimEnd());
        }
        return sb.ToString();
    }
}
```

### A.3 `AnsiScreenInterpreter` (o `TextWriter` que vira células)

```csharp
using System.Globalization;
using System.Text;

namespace ConsolePlusLibrary.Testing;

/// <summary>Interpreta o subconjunto de ANSI emitido pelo ConsolePlus e escreve no grid.</summary>
internal sealed class AnsiScreenInterpreter(VirtualScreen screen, VirtualTerminal owner) : TextWriter
{
    private enum State { Normal, Esc, Csi }
    private State _state = State.Normal;
    private bool _private;                 // sequência DEC-private (ESC[?)
    private readonly StringBuilder _params = new();
    private char _pendingHighSurrogate = '\0';

    public override Encoding Encoding => Encoding.UTF8;

    public override void Write(char value) => Feed(value);
    public override void Write(string? value) { if (value != null) foreach (char ch in value) Feed(ch); }
    public override void Write(char[] buffer, int index, int count)
    { for (int i = 0; i < count; i++) Feed(buffer[index + i]); }
    public override void WriteLine() => Feed('\n');

    private void Feed(char ch)
    {
        switch (_state)
        {
            case State.Normal:
                if (ch == '\x1b') { _state = State.Esc; return; }
                if (ch == '\n')   { screen.NewLine(); return; }
                if (ch == '\r')   { screen.CarriageReturn(); return; }
                PutChar(ch);
                return;

            case State.Esc:
                if (ch == '[') { _state = State.Csi; _private = false; _params.Clear(); return; }
                throw new NotSupportedException($"ESC {ch} não suportado pelo interpretador de teste.");

            case State.Csi:
                if (ch == '?' && _params.Length == 0) { _private = true; return; }
                if ((ch >= '0' && ch <= '9') || ch == ';') { _params.Append(ch); return; }
                Dispatch(ch, _params.ToString());
                _state = State.Normal;
                return;
        }
    }

    private void PutChar(char ch)
    {
        if (char.IsHighSurrogate(ch)) { _pendingHighSurrogate = ch; return; }
        if (_pendingHighSurrogate != '\0')
        {
            screen.Put(new Rune(_pendingHighSurrogate, ch));
            _pendingHighSurrogate = '\0';
            return;
        }
        screen.Put(new Rune(ch));
    }

    private void Dispatch(char final, string prm)
    {
        switch (final)
        {
            case 'H':   // CUP row;col (1-indexed)
            {
                var (a, b) = TwoParams(prm, 1, 1);
                screen.SetCursor(left: b - 1, top: a - 1);
                break;
            }
            case 'K': screen.EraseInLine(FirstParam(prm, 0)); break;    // EL
            case 'J': screen.EraseInDisplay(FirstParam(prm, 0)); break; // ED
            case 'm': ApplySgr(prm); break;                            // SGR
            case 'h': case 'l':
                if (_private) HandlePrivateMode(prm, set: final == 'h');
                else throw new NotSupportedException($"CSI {prm}{final} não suportado.");
                break;
            default:
                throw new NotSupportedException($"CSI {prm}{final} não suportado (surface no piloto).");
        }
    }

    private void HandlePrivateMode(string prm, bool set)
    {
        switch (prm)
        {
            case "25":   owner.SetCursorVisibleInternal(set); break;   // show/hide cursor
            case "1049": owner.SetAltScreenInternal(set); break;       // alt screen
            default: /* ignora outros modos privados irrelevantes p/ posição/estilo */ break;
        }
    }

    private void ApplySgr(string prm)
    {
        // Suporte primário: reset + truecolor (fg 38;2;r;g;b, bg 48;2;r;g;b).
        // TODO Fase 2: 4-bit (30-37/90-97, 40-47/100-107) e 256 (38;5;n / 48;5;n) para testes dessas profundidades.
        string[] p = prm.Length == 0 ? ["0"] : prm.Split(';');
        for (int i = 0; i < p.Length; i++)
        {
            int code = int.Parse(p[i], CultureInfo.InvariantCulture);
            if (code == 0) { owner.ResetColorInternal(); }
            else if ((code == 38 || code == 48) && i + 4 < p.Length && p[i + 1] == "2")
            {
                var color = new Color(
                    (byte)int.Parse(p[i + 2], CultureInfo.InvariantCulture),
                    (byte)int.Parse(p[i + 3], CultureInfo.InvariantCulture),
                    (byte)int.Parse(p[i + 4], CultureInfo.InvariantCulture));
                if (code == 38) owner.SetForegroundInternal(color); else owner.SetBackgroundInternal(color);
                i += 4;
            }
            // else: código SGR não modelado (bold/underline/…) — ignorado até D4 decidir modelar atributos.
        }
        screen.Current = new Style(owner.ForegroundRgbColor, owner.BackgroundRgbColor);
    }

    private static int FirstParam(string prm, int def)
        => prm.Length == 0 ? def : int.Parse(prm.Split(';')[0], CultureInfo.InvariantCulture);

    private static (int, int) TwoParams(string prm, int d1, int d2)
    {
        if (prm.Length == 0) return (d1, d2);
        string[] p = prm.Split(';');
        int a = p.Length > 0 && p[0].Length > 0 ? int.Parse(p[0], CultureInfo.InvariantCulture) : d1;
        int b = p.Length > 1 && p[1].Length > 0 ? int.Parse(p[1], CultureInfo.InvariantCulture) : d2;
        return (a, b);
    }
}
```

### A.4 `InputQueue`

```csharp
using System.Collections.Generic;

namespace ConsolePlusLibrary.Testing;

public sealed class InputQueue
{
    private readonly Queue<ConsoleKeyInfo> _keys = new();

    public bool HasNext => _keys.Count > 0;

    public InputQueue Enqueue(ConsoleKeyInfo key) { _keys.Enqueue(key); return this; }

    public InputQueue Enqueue(ConsoleKey key, bool shift = false, bool alt = false, bool ctrl = false)
        => Enqueue(new ConsoleKeyInfo('\0', key, shift, alt, ctrl));

    public InputQueue Type(string text)
    {
        foreach (char ch in text)
            _keys.Enqueue(new ConsoleKeyInfo(ch, CharToKey(ch), shift: char.IsUpper(ch), alt: false, control: false));
        return this;
    }

    public ConsoleKeyInfo Next() => _keys.Dequeue();

    private static ConsoleKey CharToKey(char ch)
        => char.IsLetter(ch) ? Enum.Parse<ConsoleKey>(char.ToUpperInvariant(ch).ToString())
         : char.IsDigit(ch)  ? (ConsoleKey)('0' + (ch - '0'))
         : ConsoleKey.Oem1; // VERIFY: mapeamento de pontuação conforme necessidade dos testes
}
```

### A.5 `VirtualTerminal : IConsole, IConsolePlus`

```csharp
using System.Text;
using ConsolePlusLibrary.ConsoleAbstractions; // ConsoleWriter (internal) — requer InternalsVisibleTo
using ConsolePlusLibrary.Core;                // ProfileConsole (internal)

namespace ConsolePlusLibrary.Testing;

public sealed class VirtualTerminal : IConsole, IConsolePlus
{
    private readonly VirtualScreen _screen;
    private readonly AnsiScreenInterpreter _interpreter;
    private readonly ConsoleWriter _writer;      // WRITER DE PRODUÇÃO
    private readonly ProfileConsole _profile;    // profile interno reutilizado
    private readonly InputQueue _input;
    private Color _fg, _bg;
    private bool _cursorVisible = true;
    private bool _writeToError;
    private TargetScreen _buffer = TargetScreen.Primary;

    public InputQueue Keys => _input;
    public VirtualScreen Screen => _screen;

    private VirtualTerminal(VirtualTerminalOptions o)
    {
        _fg = o.DefaultForeground; _bg = o.DefaultBackground;
        _profile = new ProfileConsole
        {
            ProfileName = "VirtualTerminal",
            IsTerminal = true,
            Interactive = o.Interactive,
            SupportsAnsi = AutoDetect.Yes,
            SupportUnicode = o.SupportsUnicode ? AutoDetect.Yes : AutoDetect.No,
            ColorDepth = o.ColorDepth,
            DefaultForegroundColor = o.DefaultForeground,
            DefaultBackgroundColor = o.DefaultBackground,
            DetectedAnsiSupport = true,
            DetectedUnicodeSupport = o.SupportsUnicode,
        };
        _screen = new VirtualScreen(o.Width, o.Height, new Style(_fg, _bg));
        _interpreter = new AnsiScreenInterpreter(_screen, this);
        _input = new InputQueue();
        _writer = new ConsoleWriter(this); // lê this.Profile.ColorDepth no ctor -> profile já pronto
    }

    public static VirtualTerminal Create(Action<VirtualTerminalOptions>? configure = null)
    {
        var o = new VirtualTerminalOptions();
        configure?.Invoke(o);
        return new VirtualTerminal(o);
    }

    // ---- callbacks internos do interpretador ----
    internal void SetForegroundInternal(Color c) => _fg = c;
    internal void SetBackgroundInternal(Color c) => _bg = c;
    internal void ResetColorInternal() { _fg = _profile.DefaultForegroundColor; _bg = _profile.DefaultBackgroundColor; }
    internal void SetCursorVisibleInternal(bool v) => _cursorVisible = v;
    internal void SetAltScreenInternal(bool alt) => _buffer = alt ? TargetScreen.Secondary : TargetScreen.Primary;

    // ---- IConsolePlus ----
    public bool WriteToErrorOutput { get => _writeToError; set => _writeToError = value; }
    public ConsoleWriter Writer => _writer;

    // ---- capacidades / perfil ----
    public IProfileReadOnly Profile => _profile;
    public IAnsiCommands Ansi => _writer.Ansi;
    public bool SupportsAnsi => true;                 // força caminho ANSI determinístico
    public bool SupportsUnicode => _profile.SupportUnicode == AutoDetect.Yes;
    public ColorSystem ColorDepth => _profile.ColorDepth;
    public int Width  => _screen.Width;               // fixo (não lê System.Console)
    public int Height => _screen.Height;
    public CancellationToken CancelToken => CancellationToken.None;
    public bool EnabledEmacs { get; set; }
    public event EventHandler<ConsoleSizeChangedEventArgs>? SizeChanged;

    /// <summary>Simula um resize para exercitar o relayout (Fase 3).</summary>
    public void RaiseResize(int newWidth, int newHeight)
    {
        // Estratégia p/ largura/altura variáveis: recriar/rebindar o grid é uma opção;
        // no mínimo, disparar o evento que BaseControlPrompt escuta.
        SizeChanged?.Invoke(this, new ConsoleSizeChangedEventArgs
        {
            Width = newWidth, Height = newHeight,
            PreviousWidth = _screen.Width, PreviousHeight = _screen.Height
        });
        // NOTE: p/ variar dimensões de verdade, tornar Width/Height mutáveis e realocar o grid. // VERIFY (Fase 3)
    }

    // ---- estilo / cor corrente ----
    public Style CurrentStyle => new(_fg, _bg);
    public ConsoleColor ForegroundColor { get => Color.ToConsoleColor(_fg); set => _fg = value; } // VERIFY ToConsoleColor
    public ConsoleColor BackgroundColor { get => Color.ToConsoleColor(_bg); set => _bg = value; }
    public Color ForegroundRgbColor { get => _fg; set { _fg = value; _writer.ApplyStyle(new Style(_fg, _bg)); } }
    public Color BackgroundRgbColor { get => _bg; set { _bg = value; _writer.ApplyStyle(new Style(_fg, _bg)); } }
    public void ResetColor() { ResetColorInternal(); }

    // ---- I/O streams: AQUI o ANSI vira células ----
    public TextWriter Out => _interpreter;
    public TextWriter Error => _interpreter;            // erros no mesmo grid (ou um 2º grid, se necessário)
    public TextReader In => TextReader.Null;
    public void SetOut(TextWriter value) { /* no-op: saída é sempre o grid */ }
    public void SetError(TextWriter value) { }
    public void SetIn(TextReader value) { }
    public bool IsInputRedirected => !_profile.Interactive;
    public bool IsOutputRedirected => false;
    public bool IsErrorRedirected => false;
    public Encoding InputEncoding { get; set; } = Encoding.UTF8;
    public Encoding OutputEncoding { get; set; } = Encoding.UTF8;

    // ---- núcleo de escrita (delegado ao writer REAL) ----
    public void Write(string? value, Style style, bool clearrestofline = false)
    { if (value != null) _writer.WriteMarkupOutput(value, style, clearrestofline); }

    public void WriteRaw(string? value, Style style, bool clearrestofline = false)
    { if (value != null) _writer.WriteOutput(value, style, clearrestofline); }

    public void WriteLine(string? value, Style style, bool clearrestofline = false)
    {
        if (value == null) return;
        Write(value, style, clearrestofline);
        _writer.WriteOutput([new Fragment("", null, FragmentKind.LineBreak)]); // VERIFY: Fragment é internal
    }

    // ---- cursor: SEMPRE via grid (read-back fiel) ----
    public void SetCursorPosition(int left, int top) => _writer.Ansi.CursorPosition(top, left); // emite CUP -> grid
    public int CursorLeft { get => _screen.CursorLeft; set => _writer.Ansi.CursorPosition(_screen.CursorTop, value); }
    public int CursorTop  { get => _screen.CursorTop;  set => _writer.Ansi.CursorPosition(value, _screen.CursorLeft); }
    public (int Left, int Top) GetCursorPosition() => (_screen.CursorLeft, _screen.CursorTop);
    public bool CursorVisible { get => _cursorVisible; set => _cursorVisible = value; }
    public bool HideCursor() { _cursorVisible = false; return true; }
    public bool ShowCursor() { _cursorVisible = true; return true; }

    public void Clear(Color? backgroundcolor = null)
    {
        if (backgroundcolor.HasValue) _bg = backgroundcolor.Value;
        _writer.Ansi.EraseInDisplay(2);
        SetCursorPosition(0, 0);
    }

    // ---- input (fila própria; NÃO usa System.Console) ----
    public bool KeyAvailable => _profile.Interactive && _input.HasNext;
    public ConsoleKeyInfo ReadKey(bool intercept = false) => _input.Next();
    public Task<ConsoleKeyInfo?> ReadKeyAsync(bool intercept, CancellationToken ct)
        => Task.FromResult<ConsoleKeyInfo?>(_input.HasNext ? _input.Next() : null);
    public string? ReadLine() => null;
    public int Read() => -1;
    public void Beep() { }

    // ---- buffers ----
    public TargetScreen CurrentBuffer => _buffer;
    public bool SwapBuffer(TargetScreen value)
    { if (value == TargetScreen.Secondary) _writer.Ansi.EnterAltScreen(); else _writer.Ansi.ExitAltScreen(); return true; }

    // ================= Overloads tipados (mecânicos) =================
    // Os overloads Write/WriteLine/WriteFormat/WriteLineFormat para char, char[], object, bool,
    // double, float, decimal, int, long e string (sem Style) NÃO têm lógica própria: apenas roteiam
    // para o núcleo Write(string?,Style,bool) / WriteLine(string?,Style,bool) usando CurrentStyle,
    // exatamente como em AnsiConsoleAdapter.cs. COPIAR VERBATIM daquele arquivo (blocos ~357–995 e
    // ~1490–1557) — não requerem substituição, pois só dependem do núcleo já implementado acima.
}
```

### A.6 `ScreenAssertions` (helpers de asserção)

```csharp
namespace ConsolePlusLibrary.Testing;

public static class ScreenAssertions
{
    public static string TextAt(this VirtualTerminal vt, int row, int col, int len)
        => vt.Screen.TextAt(row, col, len);
    public static Style StyleAt(this VirtualTerminal vt, int row, int col)
        => vt.Screen.StyleAt(row, col);
    public static string Snapshot(this VirtualTerminal vt) => vt.Screen.Snapshot();

    /// <summary>Primeira ocorrência de <paramref name="text"/> no grid, como (row, col) ou null.</summary>
    public static (int Row, int Col)? Find(this VirtualTerminal vt, string text)
    {
        for (int r = 0; r < vt.Height; r++)
        {
            string line = vt.Screen.TextAt(r, 0, vt.Width);
            int idx = line.IndexOf(text, StringComparison.Ordinal);
            if (idx >= 0) return (r, idx);
        }
        return null;
    }
}
```

### A.7 Projetos de teste (`.csproj`)

`tests/ConsolePlus.Tests/ConsolePlus.Tests.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFrameworks>net8.0;net9.0;net10.0</TargetFrameworks> <!-- VERIFY: espelhar src -->
    <IsPackable>false</IsPackable>
    <Nullable>enable</Nullable>
    <LangVersion>latest</LangVersion>
  </PropertyGroup>
  <ItemGroup>
    <!-- Decisão B (default D1): driver linkado de fonte única, compilado nesta suíte -->
    <Compile Include="..\_driver-src\**\*.cs" LinkBase="Driver" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\..\ConsolePlus\src\ConsolePlus.csproj" />
  </ItemGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="*" />
    <PackageReference Include="xunit" Version="*" />
    <PackageReference Include="xunit.runner.visualstudio" Version="*" />
    <PackageReference Include="FluentAssertions" Version="*" />
    <PackageReference Include="Verify.Xunit" Version="*" />
  </ItemGroup>
</Project>
```

`tests/PromptPlus.Tests/PromptPlus.Tests.csproj`: idêntico, trocando o `ProjectReference` para
`..\..\PromptPlus\src\PromptPlus.csproj` (que já traz o ConsolePlus transitivamente).

### A.8 `InternalsVisibleTo` (no `src` de produção)

`ConsolePlus/src/Properties/InternalsVisibleTo.cs` (ou no `.csproj`):

```csharp
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("ConsolePlus.Tests")]
[assembly: InternalsVisibleTo("PromptPlus.Tests")] // embeda o driver que toca internos do ConsolePlus
```

`PromptPlus/src/...`:

```csharp
[assembly: InternalsVisibleTo("PromptPlus.Tests")]
```

### A.9 Testes de exemplo

ConsolePlus — unidade + render de estilo (camada 1/2):

```csharp
public class StyleRenderingTests
{
    [Fact]
    public void Escreve_texto_com_cor_no_grid()
    {
        var vt = VirtualTerminal.Create(o => { o.ColorDepth = ColorSystem.TrueColor; });
        vt.WriteRaw("Ola", new Style(new Color(255, 0, 0), new Color(0, 0, 0)));

        vt.TextAt(0, 0, 3).Should().Be("Ola");
        vt.StyleAt(0, 0).Foreground.Should().Be(new Color(255, 0, 0));
        vt.CursorLeft.Should().Be(3);
    }
}
```

PromptPlus — controle via driver injetado (camada 2):

```csharp
public class SelectControlTests
{
    [Fact]
    public void Select_confirma_item_navegado()
    {
        var vt = VirtualTerminal.Create(o => { o.Width = 80; o.Height = 24; });
        var cfg = new PromptConfig();                       // internal -> InternalsVisibleTo
        var controls = new PromptPlusControls(vt, cfg);     // internal ctor

        vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Enter);

        ISelectControl<string> control = controls.Select<string>("Escolha").AddItems(["A", "B", "C"]);
        ResultPrompt<string> result = control.Run();

        result.IsAborted.Should().BeFalse();
        result.Content.Should().Be("B");
        vt.Find("B").Should().NotBeNull();
    }
}
```

> **D5 fechado:** `Select<T>()`/`Input()` retornam interfaces públicas (`ISelectControl<T>`, `IInputControl`);
> `Run(CancellationToken token = default)` retorna `ResultPrompt<T>` com `Content`/`IsAborted`. Sem `dynamic`,
> sem cast para tipo concreto interno.

### A.10 CI (GitHub Actions)

`.github/workflows/tests.yml`:

```yaml
name: tests
on: [push, pull_request]
jobs:
  test:
    strategy:
      fail-fast: false
      matrix:
        os: [windows-latest, ubuntu-latest]
    runs-on: ${{ matrix.os }}
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: |
            8.0.x
            9.0.x
            10.0.x
      - run: dotnet test tests/ConsolePlus.Tests/ConsolePlus.Tests.csproj -c Release
      - run: dotnet test tests/PromptPlus.Tests/PromptPlus.Tests.csproj -c Release
```

