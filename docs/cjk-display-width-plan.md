# Plano de ação — Largura de exibição correta para CJK (coreano/japonês/chinês)

> Audiência: mantenedores do PromptPlus. Não faz parte da documentação pública de API.

## Contexto

PromptPlus suporta 11 idiomas (`en-us, pt-br, de-de, es-es, fr-fr, it-it, ja-jp, ko-kr, nl-be, ru-ru,
zh-cn`). Em `ja-jp`, `ko-kr` e `zh-cn`, um caractere "largo" (CJK) ocupa **2 colunas** no terminal,
mas `string.Length` conta **1**. Código de layout que usa `.Length` (ou `PadRight`/`PadLeft`/slice por
índice de caractere) em vez de largura de exibição desalinha colunas, e em alguns casos pode até
cortar um rune largo no meio ou lançar exceção.

`ConsolePlusLibrary` (referenciada pelo PromptPlus) expõe os dois primitivos corretos:

- `text.GetDisplayLength()` → `int[]`, largura de exibição (colunas) de cada linha do texto.
- `text.TruncateToDisplayWidth(maxWidth)` → corta o texto por largura de colunas sem partir um rune
  largo no meio.

Ver [ADR0020V01R01](adr/ADR0020V01R01-DisplayWidthOverCharCountForLayout.md) pela decisão de
arquitetura por trás deste plano.

Auditoria feita em 2026-07-24 (todos os controles + camadas compartilhadas do PromptPlus). Status por
item abaixo.

## Causa raiz — status: **CORRIGIDO**

`ConsoleWriter.WriteOutput` (ConsolePlus) usava a largura em colunas (`GetDisplayLength`) como índice
de caractere num `Substring` cru. Provado por teste: uma string coreana de 8 chars/16 colunas num
terminal de 10 colunas lançava `ArgumentOutOfRangeException` tanto em `Overflow.Crop` quanto em
`Overflow.Ellipsis` — caminho usado por **todos** os controles via `BaseControlPrompt.WriteLineSegments`
(que força `Overflow.Ellipsis` em toda linha renderizada).

- [x] `ConsolePlus/src/Shared/StringExtensions.cs` — novo método `TruncateToDisplayWidth(maxWidth)`.
- [x] `ConsolePlus/src/ConsoleAbstractions/ConsoleWriter.cs` — `Overflow.Crop`/`Overflow.Ellipsis`
      trocados de `Substring` cru para `TruncateToDisplayWidth`.
- [x] Testes de regressão: `ConsolePlus.Tests/Rendering/OverflowTests.cs`
      (`Crop_does_not_throw_when_content_is_made_of_wide_runes`,
      `Ellipsis_does_not_throw_when_content_is_made_of_wide_runes`).
- [x] Testes de unidade: `ConsolePlus.Tests/Unit/DisplayLengthTests.cs` (6 casos novos pra
      `TruncateToDisplayWidth`).
- [x] Suíte completa `ConsolePlus.Tests` revalidada: **321/321 verde** (net10.0).

## Camadas compartilhadas do PromptPlus — status: **AUDITADO, sem bugs**

- [x] `Controls/Common/BaseControlPrompt.cs` — já correto (viewport de edição de texto via
      `GetDisplayLength`/trim por rune; corrigido em sessão de testes anterior, bug #30).
- [x] `Controls/Common/LineScreen.cs` — `ContentSize` já calculado via `GetDisplayLength().Sum()`.
- [x] `Controls/Common/BufferState.cs` — `SavePromptCursor()` e `PhysicalLineCount()` (usado pelo
      resize) já usam `ContentSize` (largura de exibição), não `.Length`.
- [x] `Controls/Common/BufferScreen.cs` — só delega para `BufferState`, sem lógica própria de largura.
- [x] `ConsolePlus/src/Shared/Fragment.cs` (usado pelos dois buffers) — só parsing de markup/cor,
      `.Length` usado apenas sobre tokens hex ASCII, sem risco.

## Consolidação de primitivo compartilhado — status: **CORRIGIDO**

Pergunta levantada durante a implementação: os métodos de viewport da classe base
(`BaseControlPrompt.ViewportSliceCore`) tinham algum bug relacionado a estas mudanças?

Resposta: não um bug de comportamento (a lógica de classificação de rune largo ali era uma **cópia
idêntica, faixa por faixa**, da de `ConsolePlusLibrary.StringExtensions`), mas um risco real de
duplicação — `PromptPlus` não tem `InternalsVisibleTo` de `ConsolePlus` (só os dois projetos de
teste têm), então a única forma de reusar a lógica seria promovê-la a `public`. Sem isso, qualquer
ajuste futuro na classificação de rune largo do ConsolePlus (ex.: os 2 desvios de especificação já
registrados na validação original) corrigiria `GetDisplayLength`/`TruncateToDisplayWidth`/tudo que os
usa, mas **não** a cópia local do viewport de edição de texto — dessincronizando o cursor/scroll do
resto do sistema.

- [x] `ConsolePlus/src/Shared/StringExtensions.cs` — `GetRuneWidth(Rune)` promovido de `private` para
      `public static int GetRuneWidth(this Rune rune)` (mudança só de visibilidade, comportamento
      idêntico).
- [x] `PromptPlus/src/Controls/Common/BaseControlPrompt.cs` — as ~30 linhas duplicadas da função local
      `RuneDisplayWidth` (dentro de `ViewportSliceCore`) removidas; agora é
      `static int RuneDisplayWidth(Rune rune) => rune.GetRuneWidth();`.
- [x] Testes novos: 3 casos em `ConsolePlus.Tests/Unit/DisplayLengthTests.cs` pra `GetRuneWidth`
      (ascii, CJK largo, marca combinante sem largura própria).
- [x] Suítes completas revalidadas em net10.0: `ConsolePlus.Tests` **324/324**, `PromptPlus.Tests`
      **677/677**.

## Centralização dos helpers — status: **FEITO**

Sugestão do usuário durante a implementação: os helpers `internal static` criados nos itens 1 e 2
(espalhados em `TableControl.cs`, `MultiTableControl.cs`, `ChartBarControl.cs`) foram centralizados
num arquivo único, `Controls/Common/DisplayWidthHelpers.cs`, classe `internal static class
DisplayWidthHelpers`:

- `Truncate(string, int)` / `AlignCell(string, int, ColumnAlignment)` — eram cópias **byte-a-byte**
  duplicadas entre `TableControl` e `MultiTableControl`; agora uma só definição, os dois controles
  chamam `DisplayWidthHelpers.Truncate(...)`/`DisplayWidthHelpers.AlignCell(...)`.
- `AlignLine(string, int, TextAlignment)`, `CountRunes(string)`, `TruncateToRuneCount(string, int)` —
  movidos de `ChartBarControl` sem mudança de lógica.
- `PadToDisplayWidth(string, int)`, `FitToDisplayWidth(string, int)` — movidos de `CalendarControl`
  sem mudança de lógica.

Decisão consciente de **não forçar unificação além disso**: `AlignCell` (usa `ColumnAlignment`) e
`AlignLine` (usa `TextAlignment`) fazem matematicamente a mesma coisa, mas os dois enums são API
pública de controles diferentes — unificá-los seria uma mudança de API maior e fora do escopo deste
plano, então ficam como dois métodos distintos no mesmo arquivo em vez de um só.

Testes consolidados também: os 3 arquivos de teste por controle (`TableDisplayWidthTests.cs`,
`ChartBarDisplayWidthTests.cs`, `CalendarDisplayWidthTests.cs`) foram substituídos por um único
`tests/PromptPlus.Tests/Unit/DisplayWidthHelpersTests.cs` — e a suíte de `MultiTableDisplayWidthTests`
(que testava a mesma lógica que `TableDisplayWidthTests`, já que os dois controles convergem pro
mesmo helper) foi removida por redundância, não só movida. Suíte completa `PromptPlus.Tests`
revalidada: **687/687 verde** em net10.0.

## Ações pendentes por controle

Ordenadas por severidade/probabilidade de uso real com dados CJK.

### 1. `TableControl.cs` / `MultiTableControl.cs` — prioridade ALTA — status: **CORRIGIDO**

Maior severidade: desalinhamento em cascata de **todas as colunas à direita** da afetada, numa
funcionalidade (tabelas) onde dado tabular CJK é um cenário realista.

- [x] `CalculateColumnWidths()` — largura mínima/auto-width de header e célula usava `.Length`;
      trocado por `GetDisplayLength()`. `TableControl.cs:1107,1114,1120-1121` /
      `MultiTableControl.cs:1296,1300,1305-1306`.
- [x] `WriteHeader()` — `headerVisibleLength`/espaço disponível calculado por `.Length`, incluindo a
      variante de preview column (esta última corrigida transitivamente, via `Truncate`/`AlignCell`).
      `TableControl.cs:1457,1478` / `MultiTableControl.cs:1172,1187`.
- [x] `Truncate(string, int)` (helper, usado por células e headers) — `value[..width]` cortava por
      contagem de caractere; agora usa `GetDisplayLength()`/`TruncateToDisplayWidth`, nunca parte um
      rune largo no meio. Promovido de `private` para `internal static` (a classe já é `internal`;
      `InternalsVisibleTo("PromptPlus.Tests")` já existia) pra permitir teste de unidade direto sem
      depender do driver de terminal virtual (que trata todo rune como 1 coluna, D4).
      `TableControl.cs:1576` / `MultiTableControl.cs:1526`.
- [x] `AlignCell(string, int, ColumnAlignment)` — mesmo padrão; agora chama `Truncate` internamente
      (elimina duplicação da lógica de corte) e calcula o padding pela largura de exibição real do
      texto já truncado. Também promovido a `internal static`.
      `TableControl.cs:1587` / `MultiTableControl.cs:1533`.

**Testes**: à época, `tests/PromptPlus.Tests/Unit/TableDisplayWidthTests.cs` (10 casos novos, unidade
pura sobre os helpers `internal static`, sem VirtualTerminal — evita a limitação D4 do driver). Suíte
completa `PromptPlus.Tests` revalidada: **677/677 verde** em net10.0 (667 prévios + 10 novos).

> `Truncate`/`AlignCell` foram depois movidos para `Controls/Common/DisplayWidthHelpers.cs` (ver seção
> "Centralização dos helpers" acima) — os `TableControl.cs:1576`/`MultiTableControl.cs:1526` etc.
> citados acima não existem mais nesses arquivos; os testes também migraram para
> `DisplayWidthHelpersTests.cs`.

### 2. `ChartBarControl.cs` — prioridade ALTA — status: **CORRIGIDO**

Trava o alinhamento vertical de **todo o gráfico** (barras de itens diferentes não começam na mesma
coluna).

- [x] `WriteLineAlign()` — lógica de padding/alinhamento extraída pra `internal static string
      AlignLine(string text, int maxWidth, TextAlignment alignment)` (testável isoladamente, sem
      `BufferScreen`), agora baseada em `GetDisplayLength`/`TruncateToDisplayWidth`. Antes, Left/Right
      podiam estourar `_width` quando o texto era mais largo em colunas do que em caracteres; agora
      trunca antes de alinhar.
- [x] `_maxLengthLabel`/label de item — ver resolução abaixo.

**Decisão tomada (2026-07-24)**: `MaxLengthLabel(byte value)` continua com a semântica pública
documentada — "número máximo de **símbolos/caracteres**" (contagem de runas), sem mudança de
contrato. O que muda é **interno**: a decisão de *quanto reter* (caracteres) e a decisão de *quantas
colunas usar no padding pra alinhar as barras* (exibição) passam a ser calculadas separadamente.

Fix concreto:
1. Corte por caractere passa a ser por **runa** (`text.EnumerateRunes()`, não por unidade UTF-16) —
   `item.Label[.._maxLengthLabel]` nunca deve partir um par substituto CJK suplementar no meio.
   Contagem de "caracteres" continua igual pra todo o BMP (inclui todo CJK das 11 línguas
   suportadas); só muda pra CJK de planos suplementares (raro).
2. Novo campo `_maxLabelDisplayWidth`, calculado uma vez em `InitControl` (ao lado de
   `_maxLengthLabel`, sobre todos os `_items`, não só a página atual): a largura de exibição real do
   **label já truncado** a `_maxLengthLabel` runas, pelo item mais largo.
3. `WriteStandardChart` passa a fazer o padding contra `_maxLabelDisplayWidth` (colunas reais),
   preenchendo manualmente com `new string(' ', _maxLabelDisplayWidth - truncatedLabel.GetDisplayLength()[0])`
   em vez de `PadRight(_maxLengthLabel)` (caracteres).

Resultado: `MaxLengthLabel(10)` continua retendo até 10 runas de qualquer label (ASCII ou CJK, sem
mudança observável), e as barras de itens ASCII e CJK no mesmo gráfico alinham na mesma coluna,
porque o padding agora reflete a largura real de cada label truncado.

**Implementado**: novo campo `_maxLabelDisplayWidth` (calculado em `InitControl`, ao lado de
`_maxLengthLabel`, sobre todos os `_items`). Novos helpers `internal static CountRunes(string)` e
`internal static TruncateToRuneCount(string, int)` (corte por runa, nunca por unidade UTF-16 —
elimina o risco de partir um surrogate pair no meio). `WriteStandardChart` agora trunca por runa e
faz o padding contra `_maxLabelDisplayWidth` (colunas reais), não contra `_maxLengthLabel`
(caracteres).

**Testes**: à época, `tests/PromptPlus.Tests/Unit/ChartBarDisplayWidthTests.cs` (8 casos novos,
unidade pura sobre `AlignLine`/`CountRunes`/`TruncateToRuneCount`, todos `internal static` — mesmo
padrão do item 1). Suíte completa `PromptPlus.Tests` revalidada: **685/685 verde** em net10.0.

> Estes 3 helpers também foram depois movidos para `Controls/Common/DisplayWidthHelpers.cs` (mesma
> nota da seção "Centralização dos helpers"); testes migrados para `DisplayWidthHelpersTests.cs`.

### 3. `CalendarControl.cs` — prioridade MÉDIA — status: **CORRIGIDO**

Cosmético mas visível: desalinha a borda da caixa do calendário em `ja-jp`/`ko-kr`/`zh-cn`.

- [x] Nome do mês via cultura — `PadRight(28)` (char-count) trocado por novo helper `internal static
      string PadToDisplayWidth(string, int)` (completa com espaços calculados pela largura de
      exibição real, nunca trunca — mesmo comportamento de antes pra nomes que já cabiam).
- [x] Abreviação de dia da semana — `PadLeft(3,' ')`/slice `abr[..3]` (char-count) trocados por novo
      helper `internal static string FitToDisplayWidth(string, int)` (trunca por `TruncateToDisplayWidth`
      sem partir rune largo, depois preenche à esquerda pela largura real).

**Testes**: `tests/PromptPlus.Tests/Unit/CalendarDisplayWidthTests.cs` (6 casos novos, unidade pura).
Suíte completa `PromptPlus.Tests` revalidada: **691/691 verde** em net10.0 (2 reexecuções; uma falha
intermitente em `ProgressBarControlTests` apareceu em ambas as rodadas completas, em métodos
DIFERENTES a cada vez, e passa isolada — flakiness de paralelismo pré-existente dos controles "Live"
[[promptplus-live-controls-strategy]], não relacionada a esta mudança; não corrigida aqui, fora de
escopo deste plano).

### 4. `SelectControl.cs` / `MultiSelectControl.cs` — prioridade BAIXA — status: **CORRIGIDO**

Cosmético, afeta só quem usa `AddSeparator(...)` com itens/grupos CJK.

- [x] `_lengthSeparationline` calculado via `item.Text.Length`/`(item.Group ?? "").Length` — trocado
      por `GetDisplayLength()` nos dois arquivos. `SelectControl.cs:405-417` (cálculo) / `:976`
      (consumo, sem alteração — só passou a receber um valor correto) — `MultiTableControl.cs`
      equivalente em `MultiSelectControl.cs:429-441` (cálculo) / `:1228` (consumo).

**Testes**: 2 casos de integração novos (via `VirtualTerminal`, não unidade pura — a lógica fica
embutida em `InitControl`/estado de instância, sem um helper estático extraível) —
`SelectControlTests.AddSeparator_line_spans_the_display_width_of_a_wide_cjk_item_not_its_character_count`
e o equivalente em `MultiSelectControlTests.cs`. Usam `AddSeparator(SeparatorLine.UserChar, '-')` com
um item CJK de 3 caracteres/6 colunas ("가나다") pra provar que a linha separadora cresce para a
largura de exibição real (6 e 10 traços respectivamente, matemática confirmada batendo exatamente com
a previsão antes mesmo de rodar). Suíte completa `PromptPlus.Tests` revalidada: **689/689 verde** em
net10.0.

### 5. `InputControl.cs` (modo Secret) — prioridade BAIXA — status: **CORRIGIDO**

Cosmético (scroll prematuro) no caso comum; pode estourar a linha só se o chamador escolher um
`_secretChar` customizado que seja um rune largo (caso raro).

- [x] `WriteAnswer()` — mascaramento trocado de `new string(_secretChar, visibleLeft.Length)` para
      calcular a largura de exibição real da fatia (`GetDisplayLength()`) e dividir pela largura do
      próprio `_secretChar` (via `GetRuneWidth()`, arredondando pra cima) — reproduz o mesmo orçamento
      de colunas que `ViewportSlice` calculou, com qualquer `_secretChar` (estreito ou largo).

**Testes**: 1 caso novo em `InputSecretControlTests.cs`
(`Typed_wide_cjk_characters_are_masked_with_one_secret_char_per_display_column`) — digita "가나다"
(3 caracteres/6 colunas) e confirma 6 `#` na máscara (não 3, o valor do bug antigo). Suíte completa
`PromptPlus.Tests` revalidada: **689/689 verde** em net10.0 (mais 1 falha intermitente isolada de
`ProgressBarControlTests`, 3ª ocorrência — sempre um método diferente, sempre passa isolado —
confirma de vez que é flakiness pré-existente de paralelismo, não relacionada a este plano).

---

## Todos os itens concluídos

Os 5 itens do plano, a consolidação dos helpers e a correção da causa raiz estão todos com status
**CORRIGIDO**. Falta só a ação de encerramento (atualizar documentação pública — ver seção acima) e,
opcionalmente, investigar a flakiness pré-existente de `ProgressBarControlTests` (fora de escopo deste
plano; ver [[promptplus-live-controls-strategy]]).

## Fora de escopo — auditado, sem bug (não precisa de ação)

`TreeControl.cs`/`MultiTreeControl.cs`, `FileControl.cs`/`MultiFileControl.cs`,
`ProgressBarControl.cs`/`TaskControl.cs`/`MultiTasksControl.cs`,
`SliderControl.cs`/`SwitchContrrol.cs`/`KeyPressControl.cs`/`TimeControl.cs`,
`MaskEditControl.cs`/`MaskEditBuffer.cs`, `FileHistory.cs`, `Paginator.cs`.

`InputControl.MaxLength` — limita número de *caracteres* digitados, é regra de negócio intencional
(como a maioria dos frameworks de UI), não uma constatação de largura visual. Não é bug, documentado
aqui só pra registrar a distinção.

## Testes

Cada item acima, ao ser corrigido, deve ganhar (no mínimo) um teste de regressão análogo aos já
criados em `OverflowTests.cs`/`DisplayLengthTests.cs`: entrada com runes largos, largura/orçamento
apertado o suficiente pra expor o bug antes do fix.

## Mudanças de API pública feitas até agora (rastreamento pra doc no encerramento)

- [x] `ConsolePlus`: `StringExtensions.TruncateToDisplayWidth(this string?, int)` — **novo método
      público**.
- [x] `ConsolePlus`: `StringExtensions.GetRuneWidth(this Rune)` — **promovido de `private` para
      público** (era `private static int GetRuneWidth(Rune)`; comportamento idêntico, só
      visibilidade).
- Sem mudança de API pública em `PromptPlus` até agora — `MaxLengthLabel(byte)` manteve seu contrato
  documentado ("número de caracteres") sem alteração observável; as promoções `private` → `internal`
  em `TableControl`/`MultiTableControl`/`ChartBarControl` não são superfície pública (as classes já
  são `internal`).

## Ação de encerramento — status: **FEITO**

`ConsolePlus/docs/api/` é **gerado automaticamente** a partir dos comentários XML pela ferramenta
DefaultDocumentation ([`ADR0015V01R01`](adr/ADR0015V01R01-GeneratedApiDocsOffLimits.md) do
ConsolePlus — edição manual desses `.md` é proibida, sempre sobrescrita no próximo build). Os 2
métodos públicos novos (`TruncateToDisplayWidth`, `GetRuneWidth`) já tinham XML doc completo desde
que foram criados/promovidos — só faltava a regeneração.

- [x] Regenerado via `dotnet build src/ConsolePlus.csproj -c Release -f net10.0` (comando oficial,
      `ConsolePlus/docs/api-documentation-guide.md`). Único arquivo alterado:
      `ConsolePlus/docs/api/StringExtensions.md` — os dois métodos novos aparecem documentados
      corretamente (assinatura, parâmetros, retorno).
- [x] Confirmado que `PromptPlus/docs/api/` **não precisa de regeneração** — nenhuma mudança deste
      plano é API pública do PromptPlus (tudo ficou `internal`: `DisplayWidthHelpers`,
      `TableControl.Truncate`/`AlignCell`, etc.).
- **Achado colateral confirmando a arquitetura**: tentar `dotnet build src/PromptPlus.csproj -c
  Release -f net10.0` agora **falha** — em Release, `PromptPlus.csproj` referencia o pacote NuGet
  *publicado* do ConsolePlus (`PackageReference`, [`ADR0019V01R01`](adr/ADR0019V01R01-ConditionalConsolePlusReference.md)),
  não o código local, e o pacote publicado obviamente não tem os métodos novos ainda. Comportamento
  esperado e documentado, não é regressão — só reforça que build Release do PromptPlus não é a via
  certa pra nada deste plano. Build Debug revalidado limpo depois.
