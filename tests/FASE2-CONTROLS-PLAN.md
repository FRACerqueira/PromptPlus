# Fase 2 — Plano de rollout controle a controle (PromptPlus)

> Só existe em `PromptPlus/tests/` (não duplicado no ConsolePlus): os 18 controles abaixo são todos
> do PromptPlus. Ao contrário do `TEST-PLAN.md` (plano geral, compartilhado entre os dois repos),
> este documento é o **tracker de progresso** desta frente específica — atualizar o status de cada
> controle ao concluí-lo, para permitir pausar/retomar entre sessões sem perder o contexto.

> **Documento histórico, congelado em 2026-07-23 no fim da Fase 2** (29 bugs reais, 972 testes —
> ver "Progresso geral" no final do arquivo). Não recebe novas entradas: achados de bugs/testes
> posteriores (auditoria da classe base `BaseControlPrompt`, extensão do driver de resize) estão em
> [`tests/TEST-PLAN.md`](TEST-PLAN.md), seção 11 ("Fase 3 — Resize") e a lista de decisões da seção
> 12. Estado atual (2026-07-24): 32 bugs reais, 980 testes.

## Como usar este documento

Cada controle recebe seu **próprio planejamento detalhado (checklist tecla×modo) só quando for a
vez dele** — não faz sentido detalhar os 18 antecipadamente (o Select/Input já mostraram que o
levantamento correto exige ler o código real, não adivinhar). Este documento define **ordem**,
**agrupamento por abordagem**, e **considerações especiais conhecidas de antemão** por leitura de
código já feita. Ao começar um controle, repete-se o processo já validado no piloto: ler
`TryResult`/`BufferTemplate` reais, montar o checklist tecla×modo, criar a(s) suíte(s) por modo,
escrever+validar os testes, documentar descobertas.

Status possíveis: `Não iniciado` / `Em andamento` / `Concluído`.

## Infraestrutura já reutilizável (não precisa redescobrir)

- **Driver VT** (`tests/_driver-src`) — pronto, maduro, sem mudanças esperadas.
- **Padrão de suíte por modo** (1 arquivo de teste por `ModeView`, ver `SelectControlTests.cs` +
  `SelectControlFilterModeTests.cs`, `InputControlTests.cs` + 2 arquivos de modo).
- **Padrão de tooltip** (F1 cicla índice, Ctrl+F1 mostra/oculta) — já testado e confirmado idêntico
  em Select e Input (`IsTooltipToggerKeyPress`/`CheckTooltipShowHideKeyPress`,
  `BaseControlPrompt.cs:827-845`). Incluir rotineiramente em todo controle novo, não é mais
  investigação, é checklist padrão.
- **Emacs através do controle** (Ctrl+A/Ctrl+E/Ctrl+K etc.) — confirmado seguro: `ConsoleHandler` é
  propriedade de instância (`BaseControlPrompt.cs:149`), então `vt.EnabledEmacs = true` no VT já
  basta, sem singleton, sem `[Collection]`. Aplica a qualquer controle cujo campo de edição/filtro
  use `EmacsConsoleBuffer` com `enableEmacsKeys` vindo de `ConsoleHandler.EnabledEmacs`.
- **`EnableHistory`** — `FileHistoryCollection` (`InputControlHistoryModeTests.cs`) já resolve a
  corrida entre classes que trocam `FileHistory.FileSystem` por `MockFileSystem`. Todo controle que
  tiver `EnableHistory` nos testes de History precisa entrar nessa mesma collection (ela já existe,
  só referenciar `[Collection(FileHistoryCollection.Name)]`).
- **Checklist "globals" de todo controle** (baseado no que já foi confirmado em Select/Input,
  verificar em cada um): tecla de abort (Escape) seta `ResultCtrl` no cancelamento (Input tinha o
  bug #8 corrigido; Switch já confirmado correto por leitura; **verificar nos demais antes de
  assumir**), Enter com predicado de validação falhando mostra erro e não confirma, tooltip F1/Ctrl+F1.

## Grupo 1 — sem `ModeView` (single-mode), aquecimento — ✅ CONCLUÍDO (2026-07-23)

Sem máquina de modos — só globais + teclas específicas do controle. 53 testes novos no total
(10+11+15+17), todos os 4 controles verdes de forma estável.

| Controle | Arquivo | Linhas | Status | Observações |
|---|---|---|---|---|
| KeyPress | `KeyPress/KeyPressControl.cs` | 307 | Concluído — 10 testes | `KeyPressControlTests.cs`. Sem bug. Cancelamento já correto. |
| Switch | `Switch/SwitchControl.cs` | 456 | Concluído — 11 testes | `SwitchControlTests.cs`. Sem bug. `EnableHistory` via `FileHistoryCollection`. |
| Slider | `Slider/SliderControl.cs` | 747 | Concluído — 15 testes | `SliderControlTests.cs`. Sem bug. Layout UpDown vs LeftRight testado. |
| ChartBar | `ChartBar/ChartBarControl.cs` | 1077 | Concluído — 17 testes | `ChartBarControlTests.cs`. **2 bugs reais corrigidos** (ver abaixo). |

**Achado do driver de teste (não é bug de produção, é gap do VT — corrigido)**: `AnsiScreenInterpreter`
não suportava SGR de 256 cores (`38;5;n`/`48;5;n`), só truecolor puro — gap já previsto e documentado
desde a decisão D4 original ("Fase 2, ver TEST-PLAN.md D4"). `SwitchStyles.Slider` (estilo padrão
`new Style(ConsoleColor.White, ConsoleColor.DarkGray)`) foi o primeiro controle a emitir essa
combinação (ajuste de contraste devolve a cor mais próxima da paleta de 256, com `.Number` setado).
Corrigido reaproveitando a tabela já testada `ConsolePlusLibrary.Core.ColorPalette.EightBit` (não
reimplementei a fórmula xterm do zero) em `tests/_driver-src/AnsiScreenInterpreter.cs`, replicado nos
dois repos.

**2 bugs reais de produção corrigidos no `ChartBarControl`**:
1. `ChartBarOrder.None` (documentado como "no sorting applied; items appear in insertion order")
   na prática ordenava por `Id` (`ChangeOrder()`, chamado incondicionalmente em `InitControl`). Como
   `AddItem` gerava `Id = Guid.NewGuid().ToString()` quando não informado, "None" **randomizava a
   ordem dos itens a cada execução** em vez de preservar a ordem de inserção. Corrigido: `None` agora
   é um no-op de verdade.
2. (Melhoria relacionada, não bug isolado, pedida pelo usuário) `Id` automático trocado de GUID pra
   um contador sequencial interno zero-padded (`_itemIdSequence`, formato `D10`) — mais estável/
   debugável, e evita quebra de ordenação lexicográfica além do 9º item caso algo mais no futuro
   ordene por `Id`.

Teste de regressão: `ChartBarControlTests.Default_order_None_preserves_insertion_order_regardless_of_item_count`.

## Grupo 2 — família `Select`/`Filter` (reaproveita o playbook do piloto)

Todos com `private enum ModeView { Select, Filter }` — mesma estrutura de 2 modos já validada em
`SelectControl`. O checklist do piloto (paginação cíclica, PageUp pousa no último item da página
anterior, filtro ativa/desativa, auto-select, navegação dentro do filtro) é o ponto de partida —
**mas cada um tem teclas/estado adicionais próprios** (multi-seleção, árvore, colunas) que exigem
levantamento específico, não é copiar-e-colar cego.

| Controle | Arquivo | Linhas | Status | Diferencial sobre o Select puro |
|---|---|---|---|---|
| MultiSelect | `MultiSelect/MultiSelectControl.cs` | 1410 | Concluído — 39 testes | Ver detalhes abaixo. |
| Table | `Table/TableControl.cs` | 1615 | Concluído — 35 testes | Ver detalhes abaixo. |
| Tree | `Tree/TreeControl.cs` | 1414 | Concluído — 26 testes | Ver detalhes abaixo. |
| MultiTable | `MultiTable/MultiTableControl.cs` | 1567 | Concluído — 35 testes | Ver detalhes abaixo. |
| MultiTree | `MultiTree/MultiTreeControl.cs` | 1500 | Concluído — 45 testes | Multi-seleção **+** árvore tri-state. Último do Grupo 2. Ver detalhes abaixo. |

Ordem sugerida dentro do grupo: MultiSelect → Table → Tree → MultiTable → MultiTree (do mais simples
pro que combina dois comportamentos já cobertos).

### MultiSelect — concluído em 2026-07-23

39 testes novos (`MultiSelectControlTests.cs` 31 + `MultiSelectControlFilterModeTests.cs` 8), split
por `ModeView` igual ao piloto. Checklist levantado lendo `TryResult`/`BufferTemplate` reais +
sondas de render (mesma técnica do piloto). Estável em 3 TFMs + 3 repetições net8, sem paralelismo
entre `MultiSelectControlTests`/`FileHistoryTests`/`InputControlHistoryModeTests` graças ao
`[Collection(FileHistoryCollection.Name)]` já reutilizado.

**Bug real #11 corrigido, achado por sonda dirigida (não pela varredura normal de teclado×modo)**:
o usuário perguntou explicitamente "o `PredicateSelected` é validado tanto para marcar quanto para
desmarcar? itens desabilitados são pulados na visão só-selecionados e o estado deles não muda?" —
ao verificar a segunda pergunta com uma sonda (item A marcado+desabilitado, B marcado, C desmarcado,
F3 depois F2), a resposta era "sim, A sobrevive corretamente" MAS a linha de resposta (`Choose: `)
ficava vazia mesmo com A ainda marcado, porque o branch de F2 dentro de `_onfilterOnlySelected`
(`MultiSelectControl.cs`, mass-uncheck que sai da visão) chamava `_answerBuffer!.Clear()`
incondicionalmente, em vez do padrão condicional (`if (_countChecked==0) Clear() else
LoadPrintable(BuildCheckedItemsText())`) usado em todo outro branch do controle. Confirmado que o
valor final confirmado (`Enter` → `Content=["A"]`) já estava correto — só a tela mentia antes do
Enter. Corrigido pra usar o mesmo padrão condicional. Teste de regressão:
`F2_from_within_the_only_selected_view_keeps_the_answer_line_in_sync_when_a_disabled_item_survives`.

**Mudança de comportamento pedida pelo usuário (2026-07-23)**: `PredicateSelected`/
`PredicateSelectedAsync` agora só é avaliado ao **marcar** um item — nunca ao desmarcar. Antes,
tanto o `Space` individual quanto os toggles em massa (`F2` dentro/fora de filtro, `Space` num
cabeçalho de grupo) chamavam o predicado nos dois sentidos, o que significava que um item já
marcado ficava **travado** (impossível desmarcar) se o predicado passasse a rejeitá-lo depois do
fato (ex.: predicado depende de estado externo mutável). Regra nova, aplicada em TODOS os pontos de
toggle do `MultiSelectControl.cs`: desmarcar nunca chama o predicado, só respeita `!Disabled`
(que já era garantido antes por outro motivo). O branch de "desmarcar tudo" dentro da visão só-
selecionados (F2 lá) já seguia essa regra por acaso (nunca chamava o predicado) — não precisou
mudar, só ficou documentado como o padrão a manter. 6 testes de regressão cobrindo os 4 pontos de
toggle (Space individual, Space em cabeçalho de grupo, F2 fora de filtro, F2 dentro de filtro) nos
dois sentidos (marcar ainda filtra pelo predicado; desmarcar ignora).

**Regra a aplicar nos próximos controles Multi\* (MultiTable, MultiTree, MultiFile)**: antes de
escrever os testes desses controles, verificar se eles têm a mesma estrutura de
predicado-nos-dois-sentidos e, se sim, aplicar a mesma correção (predicado só ao marcar, nunca ao
desmarcar; desmarcar respeita só `!Disabled`) — não é mais uma decisão de design a reconfirmar com
o usuário, é o padrão já aprovado pra família Multi\*.

**Rename pedido pelo usuário (2026-07-23), mesma sessão**: `PredicateSelected`/
`PredicateSelectedAsync` → `PredicateChecked`/`PredicateCheckedAsync` em `IMultiSelectControl<T>` +
`MultiSelectControl<T>` (as duas sobrecargas: bool puro e tupla com mensagem) — "Selected" cabia
melhor em seleção única (`Select`/`Table`/`Tree`, que MANTÊM o nome antigo, fora de escopo), mas a
operação real que o `Multi*` valida é "marcar" (checar), não "selecionar". Campo privado e helper
interno renomeados por consistência (`_predicatevalidcheck(Async)`, `TryValidateCheckPredicate`).
Testes (`MultiSelectControlTests.cs`/`MultiSelectControlFilterModeTests.cs`) e sample
(`samples/MultiSelectControlSamples/Program.cs`) atualizados; build+suíte completa revalidados
(289 testes, 3 TFMs). **Pendência criada, não resolvida agora** (usuário pediu explicitamente pra
deixar pra depois): `docs/controls/multiselect/{methods,operations,index}.md` e o guia de migração
ainda citam o nome antigo — ver [[promptplus-docs-audit]] na memória. **Mesmo rename a aplicar em
MultiTable/MultiTree/MultiFile** se/quando eles expuserem o método equivalente, ao chegar na vez
deles no rollout — ver [[promptplus-multi-predicate-rule]].

**Auditoria de nomes estendida aos demais controles já testados (2026-07-23, mesma sessão)**:
usuário pediu pra verificar se existe inconsistência de nome semelhante em Select/ChartBar/
KeyPress/Switch/Slider/Input/InputSecret. Achados e decisões:
- `Select`/`ChartBar` — `PredicateSelected` está correto (seleção única de item), sem mudança.
- `KeyPress`/`Switch` — sem método `Predicate*`, sem achado.
- **`Input`/`InputSecret`** — `PredicateSelected`/`PredicateSelectedAsync` renomeados pra
  `PredicateValid`/`PredicateValidAsync` (usuário escolheu o nome). Não tinha campo privado pra
  renomear (já eram `_predicatevalue`/`_predicatevalueAsync`/`TryInputPredicate`, nomes neutros).
- **`Slider`** — `Fill(SliderBarType type)` renomeado pra `BarType(SliderBarType type)` em
  `ISliderControl` E `ISliderWidget` (mesma mudança nos dois, `SliderControl` implementa ambos).
  Motivo: nome batia só com o valor default do enum, e o `ChartBar` já usa `BarType(ChartBarType)`
  pro mesmo conceito — inconsistência entre controles irmãos, não só interna.
- **`EnableHistory`** (presente em quase todo controle) — só **registrado como pendência**, sem
  ação: usuário confirmou manter assim por ora (escopo grande, decisão separada).
- Regra criada pra aplicar nos próximos controles sem precisar reperguntar: ver
  [[promptplus-naming-audit-checklist]] na memória.

Build completo (src+tests+os 3 samples afetados: `InputControlSamples`, `SliderControlSamples`,
`SliderWidgetSamples`) + suíte revalidados, 289 testes verdes nos 3 TFMs (antes do usuário pedir
pra simplificar validação futura pra só net10.0 — ver [[feedback-test-validation-speed]]).
**Pendência de docs também criada** pros 2 renames novos (Input/Secret, Slider), junto com a do
MultiSelect — ver [[promptplus-docs-audit]], tudo deixado pra uma passada única no final.

Diferencial confirmado sobre o Select puro (não estava óbvio antes de ler o código):
- **Checkbox extra por item**: `[x]`/`[ ]` (ASCII) entre o seletor `>`/espaço e o texto — linha
  completa é `>{indentgroup}[x] Texto` (sem espaço entre seletor e checkbox quando não há grupo).
- **`Space`** faz toggle do item focado (bloqueado por `PredicateSelected`/`PredicateSelectedAsync`,
  mesma mensagem padrão/customizada do Select). Em um cabeçalho de grupo (`IsFirstItemGroup`), o
  `Space` faz toggle de TODOS os itens do grupo de uma vez (mass-toggle, silenciosamente pula itens
  que o predicado rejeitar — sem erro).
- **`F2` (`HotKeyToggleAll`)** faz toggle de todos os itens: fora de filtro afeta a coleção inteira;
  dentro do modo `Filter` afeta só o subconjunto filtrado (`_localpaginator.AllItems()`, não
  `_items`) — comportamento realmente diferente, confirmado por sonda, não só uma suposição de
  nomenclatura.
- **`F3` (`HotKeyFilterAllSelected`)**: liga uma visão "só selecionados" que **não troca o
  `ModeView`** (fica em `Select` o tempo todo) — mecanismo paralelo ao filtro por digitação, com
  sua própria bandeira `_onfilterOnlySelected`. `F2` dentro dessa visão desmarca tudo e sai dela
  incondicionalmente (não respeita `PredicateSelected` nesse caso específico — mass-uncheck, não
  mass-check).
- **Cancelamento (Escape ou timeout de segurança) sempre retorna `Content=[]`** (array vazio) —
  diferente do Select, que preserva o item destacado no momento do cancelamento. Não é bug: Select
  devolve "o item onde a navegação parou", MultiSelect devolve "o que foi confirmado", e nada foi
  confirmado num cancelamento.
- **`Range(min,max)`**: `Enter` fora do intervalo mostra erro (`Minimum selection of N items is
  required` / `Maximum item selection(N) has been exceeded`) e NÃO confirma (mesmo padrão de erro
  de validação do Select — o loop volta a esperar tecla).
- **`ViewOnly`**: `Space`/`F2`/`F3` não têm handler ativo (todos os `else if` de toggle são
  guardados por `!_viewOnly`) — teclado ignorado silenciosamente, cai no fim do `while` sem
  `break`/`continue` e volta a esperar a próxima tecla. `Enter` confirma os itens já marcados via
  `AddItem(ischecked:true)`/`Default(...)` sem rodar `PredicateSelected`/`Range`.
- **`EnableHistory`**: salva o array de valores marcados (serializado), mas só recarrega como
  default no próximo `Run()` se `UseDefaultHistory()` for chamado explicitamente (ou `Default(...,
  useDefaultHistory: true)`, que é o default do parâmetro) — `EnableHistory` isolado, sem
  `Default`/`UseDefaultHistory`, NÃO autoaplica o histórico (`_useDefaultHistory` fica `false`).
- **Separadores** (`AddSeparator`) contam como item navegável zero (excluídos de `Qty:N items`,
  nunca respondem a `Space`, largura da linha = maior texto + largura do checkbox + 1).

### Table — concluído em 2026-07-23

35 testes novos (`TableControlTests.cs` 24 + `TableControlFilterModeTests.cs` 11). Checklist
levantado lendo `TryResult`/`BufferTemplate`/`FinishTemplate`/`LoadTooltipToggle` reais +
sondas de render. `PredicateSelected`/`PredicateSelectedAsync` mantidos sem rename (Table é
seleção única de linha — "Selected" está correto aqui, diferente do MultiSelect).

**Bug real #12 corrigido**: em modo `ViewOnly`, depois de navegar, `Enter` retornava um
`TableResult<T>` com `Value` do item INICIAL mas `RowIndex`/`ColumnIndex` da posição ATUAL do
cursor — descrevendo duas linhas diferentes dentro do mesmo resultado. Corrigido capturando
`_initialRowIndex`/`_initialColumnIndex` junto com `_initialItem` no `InitControl`, e usando os
três juntos tanto no branch de `Enter` quanto no de `Escape` (`IsAbortKeyPress`) quando
`_viewOnly=true` — por pedido do usuário, o comportamento de `Escape` em ViewOnly foi alinhado
ao de `Enter` (sempre item inicial + suas próprias coordenadas), só variando a flag `IsAborted`,
em vez de manter a inconsistência entre o que a tela mostra (`FinishTemplate` sempre exibe o item
inicial em ViewOnly) e o que era retornado.

**Bug real #13 corrigido**: `ColumnFilters` (um dos dois `FilterTableMode`) só compara o termo de
filtro contra a coluna ATUALMENTE focada (`GetColumnFilterText`). Trocar de coluna com
`Tab`/`Shift+Tab` **enquanto filtrando** — sem sair do modo filtro — fazia a tabela esvaziar
silenciosamente se a nova coluna não fosse `isFilterable` (o termo passava a comparar contra
string vazia pra toda linha). Resolvido por decisão do usuário: `Tab`/`Shift+Tab` agora sempre
saem do modo filtro (`ExitFilterMode()`, novo helper — limpa buffer+paginador, volta pra
`ModeView.Select`) antes de trocar de coluna, tanto em `Answer` quanto em `ColumnFilters` (decisão
explícita de aplicar nos dois modos, não só onde o bug existia, por consistência: "o filtro só é
válido enquanto estiver na coluna que o iniciou"). Confirmado que digitar de novo depois do Tab
inicia uma busca nova, válida, na nova coluna (não é bug reaparecendo, é o comportamento esperado
de uma busca do zero).

**Achado de UX corrigido (tooltip prometia recurso que não funcionava)**: o hint "pular pra letra"
(jump-by-letter, ativo só quando o filtro está desligado) usa `item.FilterableText`, que só é
populado a partir de colunas `isFilterable:true` — mas a tooltip aparecia **incondicionalmente**
sempre que o filtro estava desligado, prometendo um recurso que silenciosamente não fazia nada em
tabelas sem nenhuma coluna filtrável. Resolvido com resource novo e exclusivo pra família
Table/MultiTable (`PromptPlusResources.TooltipTableJump` — texto curto por pedido do usuário:
"Type initial char to jump (filterable columns)", tradução nos 11 arquivos `.resx`, incluindo
`PromptPlusResources.Designer.cs` manual), só adicionado à lista de tooltips quando
`_columns.Exists(c => c.IsFilterable)`. **Pendência pra quando chegar em MultiTable**: já usa
`TooltipTableColumnNav` (resource compartilhado) e hoje tem o MESMO problema com `TooltipJump`
genérico (linha 889 de `MultiTableControl.cs`, confirmado por leitura) — aplicar a mesma correção
(troca pra `TooltipTableJump` + condicional) lá também, sem precisar redescobrir.

**Achados de comportamento documentados (não são bugs)**:
- `GetAnswerText` (usado na linha de resposta ao vivo E no `Enter` final) cai no valor da CÉLULA
  DA COLUNA ATUAL quando não há `TextSelector` configurado — `Tab` muda qual célula aparece como
  resposta, mesmo sem o usuário ter "escolhido" aquele valor deliberadamente. A doc do
  `TextSelector` promete fallback pra `value.ToString()`, o que está desatualizado/impreciso —
  incluído na pendência de docs.
- `FilterTableMode.Answer` nunca depende de `isFilterable` (usa `GetAnswerText`, sempre funciona);
  só `ColumnFilters` tem essa dependência. Confirmado por sonda antes de aplicar a correção do Tab
  nos dois modos.
- Cancelamento por Escape REAL preserva linha/coluna atuais (como o Select); cancelamento por
  timeout (sem tecla) sempre devolve `TableResult<T>` default — mesma família de comportamento já
  vista no Select, não é bug.

Build completo + suíte revalidados nos 3 TFMs (324 testes). **Pendência de docs** (junto com as de
MultiSelect/Input/Secret/Slider já registradas): `docs/controls/table/*.md` precisa refletir o
comportamento real de `GetAnswerText`/coluna atual, o tooltip novo, e a semântica de `ViewOnly`
corrigida — ver [[promptplus-docs-audit]].

### Tree — concluído em 2026-07-23

26 testes novos (`TreeControlTests.cs` 21 + `TreeControlFilterModeTests.cs` 5). Checklist levantado
lendo `TryResult`/`InitControl`/`FinishTemplate` reais + sondas de render. `PredicateSelected`
mantido sem rename (seleção única de nó).

**Bug real #14 corrigido**: em `ViewOnly` **sem** `.Default(...)` configurado, `Enter` retornava
`Content=null` mesmo com um nó real (a Root) visivelmente selecionado na tela — o fallback só
olhava `_hasDefault ? _defaultValue : default`, sem nenhum equivalente ao `_initialItem` do Table.
Corrigido com `_initialNode` novo, capturado no fim do `InitControl` (depois do `ExpandToTarget`,
então já reflete Root OU o alvo resolvido por Default/histórico), usado como fallback no Enter em
ViewOnly.

**Bug real #15 corrigido**: rejeição por `PredicateSelected` sem mensagem customizada usava
`PromptPlusResources.SelectionDisabled` ("Item disabled") em vez de `PredicateSelectInvalid`
("Selected item does not meet the criteria") — mensagem errada, já que `Tree` não tem nenhum
conceito de nó desabilitado (`AddLast`/`AddFirst` não têm parâmetro `disable`). Corrigido só no
branch do predicado; `SelectLeafOnly` continua usando `SelectionDisabled` (esse uso faz sentido).

**Achados de comportamento documentados (não são bugs, mas divergem de outros controles)**:
- `Escape` (real ou por timeout) **sempre** devolve `Content=null`/`default`, nunca preserva a
  posição do cursor — diferente de `Select`/`Table` (que preservam no Escape real), igual ao
  `MultiSelect` (que também sempre zera). Confirmado por sonda, não é regressão nova.
- `EnableHistory` sozinho **já recarrega automaticamente** o valor salvo, sem precisar de
  `.UseDefaultHistory()`/`.Default(...)` explícito — porque `_useDefaultHistory` começa `true` por
  padrão no `Tree` (`Select`/`MultiSelect`/`Table` começam `false`, exigem opt-in). A própria doc
  do `EnableHistory` do Tree já promete esse comportamento, então não é bug — só uma
  inconsistência real entre controles, registrada aqui pra decisão futura (alinhar ou não).
- `Tab` numa linha com filhos, já expandida, **entra** no primeiro filho (navegação "drill-down");
  se ainda colapsada, expande e entra. `Shift+Tab` no primeiro filho colapsa o pai e sobe pra ele;
  em qualquer outro filho, só volta ao item anterior. Comportamento próprio do Tree, sem
  equivalente direto no Table (que usa Tab só pra colunas).
- Filtro compara contra o **caminho completo** (com Root), mas o match (`FilterMode.StartsWith`)
  bate se **qualquer segmento** do caminho comear com o termo — não precisa digitar o caminho
  inteiro desde a raiz pra achar um nó profundo.
- Resposta padrão (`_showFullPath=false`) mostra só **pai imediato + nome** (`"Apple/A1"`), não a
  cadeia completa (`"Root/Apple/A1"`, só com `ShowFullPath(true)` ou tecla `Shift+F3`) — e nem só o
  nome isolado.

Build completo + suíte revalidados nos 3 TFMs (350 testes). **Pendência de docs** (junto com as
demais): `docs/controls/tree/*.md` precisa refletir `ViewOnly` corrigido, a mensagem de predicado
corrigida, e os comportamentos de Tab/Filter/ShowFullPath acima — ver [[promptplus-docs-audit]].

### Tree — conceito de nó desabilitado adicionado em 2026-07-23 (pedido do usuário, pós-conclusão)

Usuário pediu pra avaliar o impacto de trazer o conceito de item desabilitado (já existente em
Select/Table/MultiSelect) pro Tree, usando a mesma abordagem. Avaliado sem prejuízo (Root pode ser
desabilitada sem quebrar nada — mesmo risco que Select já aceita pra um único item desabilitado) e
implementado:
- **API pública** (aditiva, `bool disable = false`): `Root`, `AddLast`, `AddFirst`, `AddAfter`,
  `AddBefore` em `ITreeControl<T>`; `AddLast`/`AddFirst` (+ `Disabled { get; }`) em `ITreeNode<T>`.
- **Modelo interno**: `TreeNode`/`VNode` privados ganharam `Disabled` (VNode é passthrough de
  `Source.Disabled`).
- **`TryResult`**: `Enter` bloqueia com `SelectionDisabled` se o nó atual está desabilitado (antes
  do `SelectLeafOnly`/predicado); todas as teclas de navegação (`Down/Up/PageDown/PageUp/CtrlHome/
  CtrlEnd/Tab/ShiftTab`) chamam um `SetSelectionDisabledErrorIfNeeded()` novo depois de mover —
  navega e mostra erro, não bloqueia a navegação. `ViewOnly` ignora `Disabled` totalmente (mesma
  exceção já dada a `SelectLeafOnly`/predicado).
- **`InitControl`**: nó inicial desabilitado mostra o erro já no primeiro render (mesmo padrão do
  Select); `Default(...)`/histórico apontando pra um nó desabilitado é ignorado na pré-seleção
  (cursor fica na raiz), mesma regra do Select/Table.
- **Render**: usa `TreeStyles.Disabled`, que **já existia no enum mas nunca tinha sido referenciado
  em `TreeControl.cs`** — achado que sugere que isso já tinha sido planejado antes e não concluído.
- **Efeito colateral no MultiTree** (`ITreeNode<T>` é compartilhado): `MultiTreeControl.TreeNode`
  precisou ganhar os mesmos membros só pra satisfazer a interface e voltar a compilar — **sem**
  expor `disable` na API pública do `IMultiTreeControl<T>` nem ligar isso no `TryResult`/render do
  MultiTree ainda (fica pra quando eu auditar o MultiTree de verdade, por pedido explícito do
  usuário: "use este conceito também no multitree quando chegar o momento").
- **6 testes de regressão** novos em `TreeControlTests.cs` (bloqueio no Enter, navegação com erro
  sem bloquear, Root desabilitada mostrando erro de cara, ViewOnly ignorando, Default pulando nó
  desabilitado, estilo visualmente distinto via `StyleAt`). Total Tree: 32 testes (26 + 6 novos).
- **Sample atualizado**: `samples/TreeControlSamples/Program.cs`, seção 18 nova (`AddLast(value,
  disable: true)` bloqueando Enter mas permitindo navegação/expansão, + `Default` apontando pra
  nó desabilitado sendo ignorado).
- **Build validado**: `TreeControlSamples` e `MultiTreeControlSamples` compilam limpo; suíte
  completa revalidada em net10.0: 313 (ConsolePlus.Tests) + 356 (PromptPlus.Tests) = **669 testes**.

**Pendência de docs marcada explicitamente pro usuário revisar**: `docs/controls/tree/*.md`
precisa de uma seção nova documentando o conceito de nó desabilitado (semântica idêntica ao
Select: visível, navegável, expansível, só não confirmável; ViewOnly ignora; Default/histórico
pulam nó desabilitado) — ver [[promptplus-docs-audit]].

### MultiTable — concluído em 2026-07-23

35 testes novos (`MultiTableControlTests.cs` 24 + `MultiTableControlFilterModeTests.cs` 11).
Checklist levantado lendo `TryResult`/`InitControl` reais + sondas de render. Como esperado,
combina exatamente os dois playbooks já validados (MultiSelect: marcar/desmarcar, F2/F3, Range,
ViewOnly; Table: colunas, Tab/ShiftTab, `GetAnswerText` por coluna atual). As 3 correções já
pré-aprovadas nas memórias (não precisou reperguntar ao usuário) foram aplicadas direto:

- **Rename**: `PredicateSelected`/`PredicateSelectedAsync` → `PredicateChecked`/
  `PredicateCheckedAsync` (`IMultiTableControl<T>` + implementação + sample). Campo privado e
  helper internos renomeados por consistência (`_predicatevalidcheck(Async)`,
  `TryValidateCheckPredicate`).
- **Bug real #16 corrigido**: igual ao #11 do MultiSelect — o `Space` individual validava o
  predicado incondicionalmente antes de decidir a direção, travando um item já marcado se o
  predicado passasse a rejeitá-lo depois. Os toggles em massa (`F2`) **já estavam certos** (só
  validam ao marcar) — só o `Space` precisou da correção. Restaurado o padrão: desmarcar nunca
  chama o predicado, só respeita `!Disabled`.
- **Bug real #17 corrigido**: igual ao #13 do Table — `ColumnFilters` só filtra pela coluna atual;
  trocar de coluna com Tab/Shift+Tab em pleno filtro esvaziava a lista silenciosamente. Corrigido
  com o mesmo `ExitFilterMode()` (Tab/Shift+Tab sempre saem do filtro antes de trocar de coluna,
  aplicado em `Answer` e `ColumnFilters` por consistência).
- **Achado de UX corrigido**: tooltip de jump-by-letter aparecia incondicionalmente (mesmo sem
  coluna filtrável) — trocado pra `PromptPlusResources.TooltipTableJump` (resource já criado pro
  Table, reaproveitado aqui) + condicionado a `_columns.Exists(c => c.IsFilterable)`.

**Investigação que NÃO confirmou bug** (registrado pra não repetir a mesma suspeita depois): o
branch de F2 "desmarcar tudo + saída da visão só-selecionados" não chama `RefreshAnswerBuffer()`
explicitamente — parecia o mesmo bug #11 do MultiSelect (linha de resposta desatualizada), mas
`WriteAnswer` já chama `RefreshAnswerBuffer()` a cada frame via `_updatePosAnswerBuffer` (resetado
pra `true` no topo de todo loop do `TryResult`), tornando as chamadas explícitas nos outros
branches redundantes, não essenciais. Confirmado por sonda antes de reportar — a tela sempre
mostrava o valor certo.

**Achado de comportamento documentado (diferença real do MultiSelect)**: a paginação só mostra o
sufixo "N seleted" quando `_countChecked > 0` — com zero itens marcados, o sufixo é **omitido
inteiramente**, não aparece como "0 seleted" (o MultiSelect sempre mostra, inclusive "0 seleted").
Testes ajustados para checar ausência de "seleted" em vez de presença de "0 seleted".

Build completo + suíte revalidada em net10.0 (391 testes). **Pendência de docs** (junto com as
demais): `docs/controls/multitable/*.md` precisa do rename + do achado do "N seleted" condicional
— ver [[promptplus-docs-audit]].

### MultiTree — concluído em 2026-07-23

45 testes novos (`MultiTreeControlTests.cs` 39 + `MultiTreeControlFilterModeTests.cs` 6). Checklist
levantado lendo `TryResult`/`InitControl`/`BufferTemplate` reais + sondas de render. As 3 pendências
herdadas (rename, predicado-só-ao-marcar, disabled) foram confirmadas e resolvidas; o gap do
`TooltipTableJump` foi investigado e **não se aplica** (MultiTree usa `_textSelector` direto pro
jump-by-letter, igual ao Tree, sem dependência de coluna filtrável).

- **Rename**: `PredicateSelected`/`PredicateSelectedAsync` → `PredicateChecked`/
  `PredicateCheckedAsync` (`IMultiTreeControl<T>` + implementação + sample). Campo privado
  renomeado por consistência (`_predicatevalidcheck(Async)`).
- **Bug real corrigido (mesmo padrão de MultiSelect/MultiTable)**: `ToggleCheck`/
  `ToggleCheckSingleNode` validavam o predicado incondicionalmente antes de calcular a direção do
  toggle. Corrigido: calcula a direção primeiro, só valida no ramo de marcar. `ToggleAllVisible`
  (F2) já estava certo nos dois sentidos.
- **Conceito de nó desabilitado implementado** (`IMultiTreeControl<T>.Root/AddLast/AddFirst/
  AddAfter/AddBefore`, todos com `bool disable = false`), com 4 decisões explícitas do usuário
  específicas do tri-state/cascata do MultiTree (diferentes do Tree de seleção única):
  1. `Space`/`Ctrl+Space` num nó desabilitado é bloqueado (`SelectionDisabled`), igual Tree.
  2. Uma cascata (`SetCheckedOnSource`) atravessa um container desabilitado sem tocar na própria
     flag dele, mas ainda alcança os descendentes habilitados (cascata "passa por, não marca").
  3. `Default(...)` força a marcação de um nó desabilitado (bypassa o bloqueio via parâmetro
     `force` novo em `SetCheckedOnSource`), igual `IMultiSelectControl<T>`.
  4. Essa marcação forçada sobrevive ao F2 de desmarcar-tudo (`ToggleAllVisible` pula nós
     desabilitados nos dois sentidos: não marca no F2-marcar-tudo, não desmarca no
     F2-desmarcar-tudo).
- **Bug real encontrado e corrigido (não fazia parte de nenhum padrão pré-aprovado, reportado ao
  usuário com evidência de sonda antes de corrigir)**: `SetCheckedOnSource` grava o próprio id de
  TODO nó tocado numa cascata (containers e folhas), mas o checkbox (`ComputeCheck`) e o contador
  do rodapé liam fontes diferentes de verdade — o checkbox de um container com `CascadeCheck=true`
  SEMPRE deriva das folhas descendentes (ignorando a própria flag), enquanto `CollectCheckedFrom`/o
  rodapé liam `_checkedSourceIds` bruto. Marcar um container em cascata e depois desmarcar UM filho
  individualmente deixava a flag do container "presa" pra sempre: a tela mostrava corretamente
  `[?]` Indeterminate, mas `Enter` incluía o container no resultado final mesmo assim, e o rodapé
  "N seleted" inflava o contador. Corrigido fazendo `CollectCheckedFrom` e o rodapé usarem a mesma
  regra do checkbox (`ComputeCheck(node) == Checked`) — tela, rodapé e resultado final nunca mais
  discordam. Efeito colateral direto (não é bug novo, é consequência do mesmo modelo agregado):
  com `RecursiveMarkWithCtrlSpace` + `CascadeCheck` (padrão, `true`), marcar só um container via
  `Space` simples (sem cascatear) fica inerte — nem aparece no checkbox, nem no resultado — porque
  o estado de um container sob cascata é sempre agregado dos descendentes, nunca uma flag própria
  independente. `Ctrl+Space` (cascata real) continua funcionando normalmente nesse mesmo cenário.
- **Ajuste fino do bug acima pra respeitar Disabled**: sem esse ajuste, um container desabilitado
  cujos descendentes ficassem 100% marcados via cascata-que-passa-por apareceria como `Checked`
  (contradizendo a decisão 2 — "passa por, não marca o próprio nó"). `ComputeCheck` agora, quando
  `source.Disabled`, só reporta `Checked` se a própria flag estiver de fato marcada (isso só
  acontece via `Default`/força — os toggles interativos nunca tocam a flag de um nó desabilitado);
  caso contrário, rebaixa um agregado `Checked` pra `Indeterminate` (nunca reivindica confirmação
  própria só por coincidência dos descendentes).
- **Render**: usa `MultiTreeStyles.Disabled` (checkbox e label), que já existia no enum mas nunca
  tinha sido referenciado em `MultiTreeControl.cs` — mesmo achado do Tree.
- **`InitControl`**: nó inicial desabilitado mostra o erro já no primeiro render, mesmo padrão do
  Tree/Select.
- **Sample atualizado**: `samples/MultiTreeControlSamples/Program.cs`, seções 22/22b novas
  (container desabilitado bloqueando `Space` mas permitindo cascata através dele; `Default`
  forçando a marcação de um nó desabilitado que sobrevive ao F2).
- **Build validado + suíte completa revalidada em net10.0**: 436 testes (PromptPlus.Tests).

**Pendência de docs marcada explicitamente pro usuário revisar**: `docs/controls/multitree/*.md`
precisa do rename, da seção nova de nó desabilitado (com as 4 semânticas específicas do
tri-state/cascata, diferentes do Tree) e da nota sobre `ComputeCheck`/`CollectCheckedFrom` — ver
[[promptplus-docs-audit]].

### MultiTree — parâmetro `check` de construção adicionado em 2026-07-23 (mesma sessão, pós-conclusão)

Usuário perguntou se dava pra ter um parâmetro de check nos nós igual ao `MultiSelect`/`MultiTable`
(`AddItem(value, ischecked:)`), com `Default` funcionando do mesmo jeito, por consistência.
Confirmado que sim e implementado, com uma complicação de arquitetura identificada e resolvida
antes de codificar (usuário escolheu explicitamente a opção "criar `IMultiTreeNode<T>`" entre 2
alternativas apresentadas):

- **Problema identificado**: `Root`/`AddLast`/`AddFirst`/`AddAfter`/`AddBefore` do
  `IMultiTreeControl<T>` só adicionam filhos diretos da raiz (ou irmãos, via `AddAfter`/
  `AddBefore`). Descer mais na árvore é feito encadeando a partir do nó retornado
  (`apple.AddLast(backend)`), e esse retorno é `ITreeNode<T>` — a interface **compartilhada com o
  `TreeControl`**, sem conceito de "check". Adicionar `check` só nos 5 métodos do controle deixaria
  de fora todo nó criado por encadeamento, que é como a maioria das árvores profundas é montada.
- **Solução escolhida**: novo tipo público `IMultiTreeNode<T> : ITreeNode<T>`
  (`src/Shared/MultiTree/IMultiTreeNode.cs`), com `AddLast`/`AddFirst` aceitando `bool check =
  false` além do `disable` já existente. `MultiTreeControl.TreeNode` (privado) passou a
  implementar `IMultiTreeNode<T>` (com forwarding explícito pra `ITreeNode<T>.AddLast/AddFirst`,
  que continuam existindo com a assinatura antiga, `check: false` implícito). Os 5 métodos do
  `IMultiTreeControl<T>` (`Root`/`AddLast`/`AddFirst`/`AddAfter`/`AddBefore`) ganharam `bool check
  = false` (parâmetro novo **no final**, depois de `disable`, pra não quebrar compatibilidade
  posicional com código existente) e os 4 que retornam nó agora retornam `IMultiTreeNode<T>` em
  vez de `ITreeNode<T>`. `ITreeNode<T>`/`TreeControl` ficam **intocados**.
- **Semântica implementada** (confirmada por sonda antes de escrever os testes finais):
  `check: true` é **aditivo** com `Default(...)`/histórico — nenhum dos dois limpa o outro,
  aplicado via `ApplyConstructionTimeChecks` no `InitControl`, chamado ANTES da resolução de
  `Default`/histórico, reaproveitando o mesmo `SetCheckedOnSource(node, true, force: true)` já
  usado pelo `Default`. Um container com `check: true` cascateia pros descendentes igual a um
  check interativo (respeitando `CascadeCheck`). Combinado com `disable: true`, `check` força a
  marcação através do bloqueio, igual `Default` já fazia pra nó desabilitado. **Diferente do
  `Default`**: `check` não auto-expande a árvore até o nó — é uma flag de dado inicial silenciosa,
  não uma dica de UX "olhe aqui".
- **6 testes de regressão novos** em `MultiTreeControlTests.cs` (check em nó encadeado sem
  auto-expandir, check em folha encadeada confirmado no Enter, check em container cascateando,
  check na própria raiz cascateando, check aditivo com Default, check + disable forçando através
  do bloqueio). Total MultiTree: 51 testes (45 + 6 novos).
- **Sample atualizado**: `samples/MultiTreeControlSamples/Program.cs`, seções 23/23b novas (nó
  encadeado com `check: true` sem auto-expandir; container com `check: true` cascateando +
  composição aditiva com `Default`).
- **Build validado + suíte completa revalidada em net10.0**: 442 testes (PromptPlus.Tests).

**Pendência de docs marcada explicitamente pro usuário revisar**: `docs/controls/multitree/*.md`
precisa de uma seção nova sobre `check` (incluindo o tipo novo `IMultiTreeNode<T>` e a mudança de
tipo de retorno de `AddLast`/`AddFirst`/`AddAfter`/`AddBefore`) — ver [[promptplus-docs-audit]].

## Grupo 3 — edição mascarada

| Controle | Arquivo | Linhas | Status | Observações |
|---|---|---|---|---|
| MaskEdit | `MaskEdit/MaskEditControl.cs` | 1865 | Concluído — 73 testes | Ver detalhes abaixo. |

### MaskEdit — concluído em 2026-07-23

`MaskEditControl<T>` não tem `ModeView` — uma única classe genérica despacha comportamento por
tipo em runtime (string/int/long/decimal/double/DateTime/DateOnly/TimeOnly), implementando 4
interfaces fluentes (`IMaskEditStringControl`/`IMaskEditNumberControl`/
`IMaskEditCurrencyControl`/`IMaskEditDateTimeControl`). Suíte dividida por TIPO em vez de modo —
73 testes novos: `MaskEditStringControlTests.cs` (24, cobre também os comportamentos
COMPARTILHADOS por todos os 4 tipos — Enter/Escape/tooltip/Default/predicado — só uma vez, para
não duplicar 4x), `MaskEditNumberControlTests.cs` (17), `MaskEditCurrencyControlTests.cs` (14),
`MaskEditDateTimeControlTests.cs` (18).

**4 bugs reais corrigidos**:
- **Bug real**: `U[...]`/`{U[...]}` (letra maiúscula customizada) aceitavam minúscula
  indevidamente — `NormalizeStringMask` setava `Validchars = CharLowerLetters` (deveria ser
  `CharUpperLetters`) nos dois branches de customização (single-char e grupo). O branch `U` SEM
  customização (mask simples) já estava certo. Confirmado por sonda: `U[AB]`, digitar `z`
  (minúscula, fora de `[AB]`) era aceito e retornado. Corrigido nos 2 pontos.
- **Bug real**: ciclo de tooltip (F1) anunciava incondicionalmente os 7 atalhos Emacs
  (`MaskEditBuffer.GetEmacsTooltips()`), mesmo com `ConsoleHandler.EnabledEmacs=false` (padrão) —
  diferente de todo outro controle, que usa `GetEmacsTooltips(bool)` já gated nesse mesmo flag. O
  gating da FUNCIONALIDADE em si (`TryAcceptedReadlineConsoleKey`) já existia antes desta sessão
  e não foi alterado — só o tooltip estava fora de sincronia. Corrigido adicionando o parâmetro
  `enabledEmacs` ao método e passando `ConsoleHandler.EnabledEmacs` no call-site. Discutido com o
  usuário 3 alternativas (manter ligado ao flag global / sempre false / flag própria
  desacoplada) — usuário confirmou manter ligado ao flag global após eu verificar por sonda que
  as 7 teclas (Ctrl+L/H/E/A/B/F/D) já funcionavam corretamente quando `EnabledEmacs=true`.
- **Bug real, mais grave**: sinal negativo digitado era descartado silenciosamente no resultado
  de `MaskInteger`/`MaskLong` — `MaskEditBuffer.GetWithoutMask()` pro ramo `s_isIntegerNumber`
  nunca olhava pro elemento `SignSymbol`, só concatenava `InputMask`/`InputConstant` (o ramo
  decimal/double já fazia certo, com `IsNegative`/`IsPositive`). Confirmado por sonda: digitar
  `-5` num `MaskInteger` com `NumberFormat(..., withsignal: true)` renderizava `"- __5."`
  corretamente mas `Enter` devolvia `5`, não `-5`. Corrigido espelhando o ramo decimal.
- **2 typos em resource** (neutro/inglês): `TooltipJumpdelimiter` = `"Tab/ShitTab:Jumps between
  delimiters "` (faltava o 'f', e usava `/` em vez de `\` como todo outro idioma) e
  `MaskEditPosLetterLower` = `"Letra (a-z)"` (palavra em português esquecida, deveria ser
  `"Letter (a-z)"` — só o `pt-BR.resx` deveria ter essa palavra). Corrigidos nos `.resx` neutro +
  `pt-BR.resx` (só o typo do separador/letra faltante) + comentário de doc no `Designer.cs`.

**Melhoria de validação eager (não é bug, é fail-fast)**: `NumberFormat(...)` agora valida os
limites de dígitos (10/19/28/15 pra int/long/decimal/double) imediatamente na própria chamada,
em vez de esperar o `Run()` normalizar a máscara pra descobrir o mesmo limite via
`CountNumericMask`. `integerpart`/`decimalpart` já são conhecidos no ponto de chamada — não
precisa esperar a máscara ser normalizada pra re-descobrir a mesma contagem. As checagens antigas
em `NormalizeNumberMask` ficaram como estão, como backstop defensivo (nunca mais alcançadas na
prática).

**Achados de comportamento não óbvios, confirmados por sonda** (documentados nos comentários de
cabeçalho de cada arquivo de teste, não repetidos aqui):
- `new PromptConfig()` usa `CultureInfo.CurrentCulture` (locale da máquina) como
  `DefaultCulture`, não invariant — todo teste de Number/Currency/DateTime precisa fixar
  `.Culture("en-US")` explicitamente pra ter separadores/ordem de data determinísticos.
  `PromptConfig` "esquece" de resetar `Thread.CurrentThread.CurrentCulture` depois do `Run()`
  (`BaseControlPrompt.Run()` seta e nunca restaura) — não é bug do MaskEdit especificamente
  (afeta todo controle), fora de escopo, mas motivo pra nunca usar `result.Content.ToString()`
  implícito em asserção de teste (usar igualdade estrutural).
- Máscara numérica sempre digita via "shift-left" a partir da posição do separador decimal — pra
  máscaras COM parte decimal (`NumberFormat(int, dec)`), digitar sem navegar preenche só a parte
  INTEIRA (o `Shiftleft` para de escanear no `DecimalSeparator`, nunca inclui posições
  decimais); pra preencher a parte decimal, o usuário precisa mover o cursor pra direita
  explicitamente (seta ou Ctrl+F) e digitar direto lá (fill posicional normal, não shift).
  Fluxo esperado: digita a parte inteira (shift automático), move pra direita, digita a parte
  decimal.
- Digitar mais dígitos do que cabem na parte inteira, uma vez preenchida por completo, é
  simplesmente REJEITADO (não desloca o dígito mais antigo pra fora) — `Shiftleft` só desloca
  quando existe pelo menos uma posição vazia pra "puxar" o novo dígito.
- Máscara de data/hora reordena dia/mês/ano conforme `culture.DateTimeFormat.ShortDatePattern`,
  mas o SEPARADOR final é sempre `/` literal, independente do separador de data da cultura
  (`culture.DateTimeFormat.DateSeparator` só é usado pra fazer o SPLIT do padrão, nunca
  reaproveitado na reconstrução). Ordem da parte de HORA (`h:m:s`) nunca é reordenada por
  cultura.
- `Tab`/`Shift+Tab` pulam pro próximo/anterior campo em máscaras de data/hora (usando
  `JumpNextDelimiter`/`JumpPreviusDelimiter`), mas são totalmente inertes (no-op) em máscaras de
  string e numéricas — confirmado por sonda nos 2 casos.

**Sample**: os 4 projetos (`MaskEditStringControlSamples`, `MaskEditNumberControlSamples`,
`MaskEditCurrencyControlSamples`, `MaskEditDateTimeControlSamples`) não precisaram de nenhuma
alteração — nenhum dos 4 bugs mudou a API pública, só corrigiu comportamento interno já
documentado/esperado (inclusive a seção 3 do sample de Number já demonstra
`NumberFormat(3, withsignal: true)` pra "Temperature", que estava silenciosamente quebrada antes
do fix). Build dos 4 confirmado limpo.

**Build validado + suíte completa revalidada em net10.0**: 515 testes (PromptPlus.Tests).

## Grupo 4 — modos próprios (não é Select/Filter)

| Controle | Arquivo | Linhas | Status | Observações |
|---|---|---|---|---|
| Calendar | `Calendar/CalendarControl.cs` | 1354 | Concluído — 41 testes | Ver detalhes abaixo. |

### Calendar — concluído em 2026-07-23

`CalendarControl` tem `ModeView { Input, ShowNotes }` — mas ao contrário de Select/Table (filtro
por digitação), `ShowNotes` é um painel de notas do dia com sua própria paginação, entrado/saído
via `F2` (só quando o dia atual tem nota). 41 testes novos: `CalendarControlTests.cs` (30, modo
`Input` — navegação de dia/semana/mês/ano, Range, disabled/weekend, highlights/notas, predicado,
histórico) + `CalendarControlNotesModeTests.cs` (11, modo `ShowNotes`). Checklist levantado lendo
`TryResult`/`InitControl`/`BufferTemplate` reais + sondas de render. **Nenhum bug real encontrado**
neste controle — todos os comportamentos batem exatamente com o que o código já implementava.

Achados de comportamento não óbvios, confirmados por sonda (documentados nos comentários de
cabeçalho dos 2 arquivos de teste):
- Navegar (Tab/ShiftTab/PageUp/PageDown/setas/Home) marca `_selectedDate = _currentDate` sempre
  que a data é válida (`IsValidSelect`), **sem** reavaliar o predicado — o predicado só é checado
  no `Enter` (`ValidateSelection`). Já a seleção INICIAL (`InitControl`) respeita o predicado desde
  o primeiro render — se o predicado rejeita a data padrão, `_selectedDate` já nasce nulo e `Enter`
  mostra "Invalid date selected!" (a mensagem genérica, não a do predicado, que nunca chega a ser
  avaliada nesse caso específico). Pra exercitar a mensagem do predicado
  (`PredicateSelectInvalid`) é preciso navegar até uma data especificamente rejeitada
  DEPOIS da inicial.
- O buffer de texto (`EmacsConsoleBuffer`) usado pra exibir a nota selecionada no modo `ShowNotes`
  é somente-leitura de fato — digitar uma letra imprimível nunca edita o texto, pula
  (jump-by-letter, com wraparound) pra próxima nota que comece com essa letra. Só teclas de
  navegação (setas/Home/End) movem o cursor dentro do viewport da nota exibida.
- `Enter` dentro do modo `ShowNotes` primeiro reseta pro modo `Input` e ENTÃO continua o fluxo
  normal de confirmação (usa `_selectedDate`, que nunca mudou enquanto via notas) — ou seja, fecha
  as notas E confirma o calendário no mesmo pressionar de tecla.
- `EnableHistory` segue a convenção da família Select/Table (`_useDefaultHistory` começa `false`)
  — sozinho não recarrega; precisa de `Default(valor, useDefaultHistory: true)` (o `true` é o
  default do parâmetro) pra habilitar o auto-reload, mesmo que o valor de `Default` em si seja
  descartado pelo histórico.
- Setas de navegação de dia (`Left/Right/Up/Down`) já respondem aos equivalentes Emacs
  (`Ctrl+B/F/P/N`) **incondicionalmente**, sem depender de `ConsoleHandler.EnabledEmacs` — é o
  parâmetro `emacskeys` default `true` do extension method compartilhado
  (`ConsoleKeyInfoExtensions`), não uma flag de configuração do Calendar.
- Glifos ASCII confirmados por sonda: nota=`*`, destaque(`Highlights`)=`!`, nota+destaque juntos
  usam um terceiro glifo combinado.

**Achado de teste, não é comportamento do controle**: strings de tooltip Emacs completas (ex.
`Emac_ctrl_b`) são mais longas que a largura padrão do terminal e ficam cortadas na renderização —
os testes que checam esses tooltips usam um prefixo curto (`vt.Find("Ctrl+B:Moves the cursor
back")`) em vez do resource inteiro.

**Sample**: `CalendarControlSamples` não precisou de nenhuma alteração (nenhum bug encontrado).
Build confirmado limpo.

**Build validado + suíte completa revalidada em net10.0**: 556 testes (PromptPlus.Tests).

## Grupo 5 — acesso a filesystem real

**Atualizado em 2026-07-23 — abstração `IFileSystem` implementada** (decisão revista: a "alternativa
descartada" original virou a abordagem escolhida). `FileControl`/`MultiFileControl` agora têm
`internal static IFileSystem FileSystem { get; set; } = new FileSystem();` (mesmo padrão do
`FileHistory.FileSystem`), com todos os pontos que tocavam `Directory`/`File`/`DirectoryInfo`/`FileInfo`
diretamente (6 em `FileControl.cs`, 14 em `MultiFileControl.cs`) migrados pra `FileSystem.Directory.*`/
`FileSystem.File.*`/`FileSystem.Path.*`/`FileSystem.DirectoryInfo.New(...)`/`FileSystem.FileInfo.New(...)`
— confirmado por reflection que `IDirectory`/`IPath`/`IDirectoryInfoFactory`/`IFileInfoFactory` cobrem
100% dos overloads usados (inclusive `EnumerateDirectories(string,string,EnumerationOptions)`). Os
helpers de manipulação pura de string (`Path.DirectorySeparatorChar`, `Path.GetFileName` etc., em
`CountSeparators`/`EnsureTrailingSeparator`/`PathEquals`/`IsUnixHiddenByName`) foram **deixados como
`System.IO.Path` puro, deliberadamente** — não tocam disco, então `MockFileSystem` não mudaria o
comportamento ali. Nenhuma mudança na API pública (`IFileControl`/`IMultiFileControl` intactas); build
+ suíte inteira (197 testes) revalidados verdes depois da migração.

**Achado confirmado por reflection que justificou a abordagem**: `MockFileSystem.StringOperations.Comparer`
é sempre `OrdinalIgnoreCaseComparer` e a versão instalada (`TestableIO...TestingHelpers 22.2.0`) não
expõe nenhuma forma de configurar isso — o mock nunca reproduz a semântica case-sensitive real do
Linux. Mitigação: `FileControlRealFilesystemTests.cs` (2 testes, contra diretório temporário real via
`Directory.CreateTempSubdirectory`, sem trocar `FileSystem` — usa o real de propósito) cobre
especificamente esse ponto; o resto da suíte (navegação, expand/collapse, tooltip, histórico) pode
usar `MockFileSystem` com segurança. Symlinks foram descartados como risco: nenhum dos dois controles
tem lógica própria de symlink/reparse point (confirmado por leitura, é enumeração simples).

| Controle | Arquivo | Linhas | Status | Observações |
|---|---|---|---|---|
| FileControl | `FileExec/FileControl.cs` | 1105 | ✅ Concluído — 18 testes | `FileControlTests.cs` (16, Mock-based) + `FileControlRealFilesystemTests.cs` (2, sanity real-disk). Nenhum bug real encontrado. |
| MultiFileControl | `MultiFile/MultiFileControl.cs` | 2397 | ✅ Concluído — 26 testes | `MultiFileControlTests.cs` (23, Mock-based) + `MultiFileControlRealFilesystemTests.cs` (3, sanity real-disk). Nenhum bug real encontrado; só a renomeação padrão `PredicateSelected`→`PredicateChecked` (já aplicada, ver [[promptplus-multi-predicate-rule]]). |

### FileControl — concluído em 2026-07-23

Sem `ModeView` — é uma árvore single-select (Tab/Shift+Tab drill down, `+`/`-` expand/collapse,
jump-by-letter, `SearchPattern` só filtra arquivos, `ShowFullPath` no hotkey `Shift+F3` — não `F3`
puro). `_useDefaultHistory = true` por padrão (convenção de Tree/MultiTree, diferente de
Select/Table/Calendar): `EnableHistory` sozinho já recarrega o último caminho confirmado, sem
precisar de `Default(...)` explícito. 18 testes: `FileControlTests.cs` (16, Mock-based) +
`FileControlRealFilesystemTests.cs` (2, pré-existente, sanity real-disk pra case-sensitivity no
Linux). **Nenhum bug real encontrado.**

### MultiFileControl — concluído em 2026-07-23

O controle mais complexo do rollout até agora: checkboxes tri-state (`Selected`/`PartialSelect`/
`NotSelect`) computados só a partir dos descendentes **visíveis** (`_nodes`), mais um mecanismo de
seleção recursiva de pasta em **background** (`Space`/`Ctrl+Space` numa pasta desmarcada com
`CascadeCheck=true` dispara `Task.Run` que enumera o subtree real em disco fora da UI thread,
aplicando o predicado se houver). Marcar-tudo-visível (F2), filtro "só selecionados" (F3, view flat
separada de `_nodes`), `Range`, `CascadeCheck(false)`/`RecursiveMarkWithCtrlSpace` (bifurcam se
`Space` recursa ou só alterna o item), e `PredicateChecked`/`PredicateCheckedAsync` (Option C: toggle
individual mostra erro, seleção em massa pula silenciosamente) — tudo lido do código real e
confirmado por sonda antes de travar os testes. **Nenhum bug real encontrado** — inclusive a lógica
de direção do predicado (`ToggleCheckedWithPredicate`) já calculava `willCheck` corretamente antes
desta sessão, ao contrário do que precisou ser corrigido em MultiSelect/MultiTable/MultiTree.

**Técnica de teste nova, reutilizável para qualquer controle assíncrono futuro**: como
`vt.Keys.Enqueue(...)` deixa todas as teclas da fila imediatamente "disponíveis" (sem modelo de
tempo real), enfileirar `Space` (marca uma pasta) seguido de `Enter` no mesmo lote faz o `Enter`
disparar ANTES da tarefa de fundo terminar (confirmado por sonda: `Count=0`). A correção é rodar
`control.Run(cts.Token)` numa `Task.Run(...)`, dar um `Thread.Sleep(400)` na thread do teste (tempo
de relógio real pra tarefa de fundo terminar), SÓ ENTÃO enfileirar a próxima tecla na fila ainda
viva, e finalmente `runTask.GetAwaiter().GetResult()`. Pra cenários que precisam capturar o estado
"ainda rodando" de forma determinística (glifo de espera, cancelar reapertando a tecla), um
predicado propositalmente lento (`Thread.Sleep(1000)` dentro do `PredicateChecked`) garante uma
janela grande e confiável — contar com o timing natural de um `MockFileSystem` de 1 arquivo (rápido
e imprevisível) para "ainda não terminou" se mostrou flaky em teste real (uma sonda com só
`Thread.Sleep(150)` pegou "ainda rodando" numa execução e "já terminou" na seguinte).

**Achado de resource compartilhado, não é bug introduzido nesta sessão — corrigido a pedido do
usuário**: `TooltipCountCheck` ("`{0} seleted.`") tinha o typo "seleted" em vez de "selected" —
string usada por TODOS os controles `Multi*` (MultiSelect/MultiTable/MultiTree/MultiFile), já
presente antes desta sessão e já "travada" nos testes das suítes concluídas em Grupo 2. Reportado
ao usuário (corrigir cascatearia pelas asserções já validadas de Grupo 2); usuário pediu pra
corrigir mesmo assim. Corrigido em `PromptPlusResources.resx`/`Designer.cs` (só o neutro/inglês —
as traduções nos outros `.resx` já estavam certas) + todas as asserções afetadas em
`MultiSelectControlTests.cs`, `MultiSelectControlFilterModeTests.cs`, `MultiTableControlTests.cs`,
`MultiTableControlFilterModeTests.cs`, `MultiTreeControlTests.cs` (`"N seleted"` → `"N selected"`).
Build + suíte completa revalidados verdes (598 testes) depois da correção.

**2º achado nesta mesma rodada de validação (flakiness de teste, não é bug de produção)**:
`MultiFileControlRealFilesystemTests.Checking_a_folder_recursively_walks_the_real_disk_subtree_in_the_background`
falhou uma vez rodando a suíte inteira em paralelo (`Thread.Sleep(400)` não foi margem suficiente
sob contenção real de thread-pool com ~600 testes rodando juntos), mas passou isolado. Corrigido
aumentando a margem fixa pra `Thread.Sleep(1500)` — é um teste de sanity real-disk, roda uma vez só,
margem maior não custa nada na prática. Revalidado verde em 2 execuções completas seguidas da
suíte inteira depois do ajuste.

26 testes: `MultiFileControlTests.cs` (23, Mock-based) + `MultiFileControlRealFilesystemTests.cs` (3,
sanity real-disk, incluindo um teste que exercita `StartBackgroundWildcard`/`EnumerateSubtree` contra
disco de verdade).

**Sample**: `MultiFileControlSamples` não usa `PredicateSelected` — nenhuma alteração necessária pela
renomeação. Build confirmado limpo.

**Build validado + suíte completa revalidada em net10.0**: 598 testes (PromptPlus.Tests).

## Grupo 6 — controles "Live" (`IsLiveAutoRenderControl => true`)

Tela muda **sem** depender de tecla do usuário (auto-render). Estratégia decidida em 2026-07-23
(ver [[promptplus-testing-plan]] pra rationale completo): **não existe uma estratégia única pro
grupo** — cada controle tem um motor de conclusão diferente:
- `Time`: `Stopwatch` real puro (wall-clock), sem nenhum hook externo — é o caso mais difícil, por
  isso foi deixado para o fim do grupo por pedido do usuário ("exclua o Timer dos testes por
  enquanto").
- `ProgressBar`/`TaskExec`/`MultiTasks`: exigem um callback fornecido pelo CHAMADOR
  (`UpdateHandler(Async)`/ação do task) rodando numa `Task` de fundo — o TESTE escreve esse
  callback, então a conclusão é determinística (um handler que atualiza o valor pro máximo e
  retorna termina o controle quase instantaneamente, sem depender de `Duration`/`Sleep`
  adivinhados).

**Ordem escolhida**: ProgressBar primeiro (mais simples dos 3 "callback-driven", estabelece o
padrão pros outros dois); Time fica pro final do grupo.

| Controle | Arquivo | Linhas | Status |
|---|---|---|---|
| ProgressBar | `ProgressBar/ProgressBarControl.cs` | 861 | ✅ Concluído — 22 testes, 3 bugs reais |
| TaskExec | `TaskExec/TaskControl.cs` | 600 | ✅ Concluído — 15 testes, 3 bugs reais |
| MultiTasks | `MultiTasks/MultiTasksControl.cs` | 882 | ✅ Concluído — 15 testes, 1 bug real |
| Time | `Time/TimeControl.cs` | 477 | ✅ Concluído — 9 testes, nenhum bug real |

### ProgressBar — concluído em 2026-07-23

22 testes novos (`ProgressBarControlTests.cs`). **3 bugs reais corrigidos** (todos reportados com
evidência de sonda antes de corrigir):
- **Erro no handler classificado como sucesso**: `TryResult` checava o genérico
  `ProgressBarEvent.Finish` (`true` tanto pra sucesso quanto pra erro/abort, já que
  `ErrorAndAbort` seta a mesma flag `_aborted`) ANTES do branch específico de erro — uma exceção
  no handler sempre resultava em `IsAborted=false` (parecia sucesso), mesmo com
  `StateProgress.ExceptionProgress` corretamente preenchido. Corrigido invertendo a ordem dos dois
  `if` (erro/abort checado primeiro).
- **Texto do prompt final sem o separador `": "`**: `FinishTemplate` escrevia
  `OptionsControl.PromptValue` direto, sem aplicar `SufixAfterPromptValue` (o que `WritePrompt` —
  usado em todo outro lugar, inclusive no `BufferTemplate` deste mesmo controle — já faz).
  Resultado: `"Working100%"` em vez de `"Working: 100%"`. Corrigido chamando `WritePrompt` em vez
  de reimplementar a lógica.
- **`HideElements(HideProgressBar.ElapsedTime)` ignorado no frame final**: honrado em
  `WriteAnswer` (enquanto roda) mas não em `FinishTemplate` (o frame final sempre mostrava o
  elapsed time). Corrigido com a mesma checagem de flag usada em `WriteAnswer`.

**Técnica de teste nova pra "meio do caminho" determinístico**: em vez de adivinhar um
`Thread.Sleep`, o handler seta o valor, sinaliza um `ManualResetEventSlim` ("ready") e BLOQUEIA
esperando outro sinal ("proceed") liberado pelo teste. O teste espera o "ready" (confirmação real,
não suposição de tempo), dá uma margem fixa pequena só pro loop de render notar a mudança e
repintar, lê o snapshot, e libera o "proceed". `WriteDescription`/`WriteTooltip` só existem no
`BufferTemplate` (nunca no `FinishTemplate`) — testar `ChangeDescription`/F1 exige essa técnica,
não dá pra observar no frame final.

**Achado de flakiness sob carga paralela, corrigido com uma técnica melhor que só aumentar
sleep**: rodando a suíte inteira (620 testes, várias classes em paralelo por padrão do xUnit),
tanto `ProgressBarControlTests` quanto `MultiFileControlRealFilesystemTests` (que já usava a
técnica de tarefa de fundo do Grupo 5) flakaram intermitentemente sob contenção pesada de
thread-pool — sempre passando isolados, falhando ocasionalmente só na suíte completa. Em vez de só
aumentar margens de `Thread.Sleep` (o que já tinha sido tentado pro MultiFile, 400ms→1500ms, e
ainda flakava), a correção certa foi criar `BackgroundTimingCollection`
(`[CollectionDefinition(Name, DisableParallelization = true)]`, mesmo padrão do
`FileHistoryCollection` já existente) e colocar as duas suítes nela — isso serializa essas classes
ESPECÍFICAS entre si (reduzindo a contenção real: várias tarefas de fundo brigando por thread-pool
ao mesmo tempo), sem tornar a suíte inteira sequencial. Com o isolamento aplicado, as margens
puderam ser reduzidas de volta (handshake `ManualResetEventSlim`: 80ms→30ms; teste real-disk do
MultiFile: 3000ms→500ms) e a suíte ficou estável em múltiplas rodadas completas seguidas.

Sample: não existe `ProgressBarControlSamples` dedicado nesse levantamento — nenhuma alteração de
amostra necessária (nenhum bug mudou API pública).

**Build validado + suíte completa revalidada em net10.0, estável em 3 execuções completas
seguidas após o ajuste de paralelismo**: 620 testes (PromptPlus.Tests).

### TaskExec — concluído em 2026-07-23 (mesma sessão)

15 testes novos (`TaskControlTests.cs`, já em `[Collection(BackgroundTimingCollection.Name)]`
desde o início, sem precisar descobrir a flakiness de novo). Mesma família do `ProgressBar`
(callback do chamador — `Action(Async)` — rodando numa `Task` de fundo, `_completed`/`_error`
sinalizam conclusão), então a técnica de teste (handler síncrono determinístico + handshake
`ManualResetEventSlim` pra estado "em andamento") se transferiu direto. **3 bugs reais
corrigidos** (reportados com evidência de sonda, usuário aprovou os 3 de uma vez):
- **Descrição/tooltip colavam na linha do prompt**: `WriteAnswer` só chamava
  `screenBuffer.WriteLine("")` (terminando a linha) quando havia elapsed time ou spinner pra
  mostrar — na configuração PADRÃO (nenhum dos dois), a linha nunca era terminada, e
  `WriteDescription`/`WriteTooltip` acabavam colados na mesma linha do prompt
  (`"Working: F1:Tips.Esc:Abort."` em vez de duas linhas). Corrigido terminando a linha
  incondicionalmente.
- **Campo de opção errado pro texto de abort sem erro**: `FinishTemplate` checava
  `OptionsControl.EnabledAbortKeyValue` (se a tecla de abort está habilitada) em vez de
  `ShowMessageAbortKeyValue` (se deve mostrar mensagem de cancelamento), diferente do `ProgressBar`
  pro mesmo conceito. Código morto como estava escrito (só alcançável via Escape, que já exige
  `EnabledAbortKeyValue=true` pra disparar), mas corrigido por consistência.
- **`OperationCanceledException` lançada pelo próprio handler era reportada como sucesso**: o
  catch específico marcava corretamente "não é erro" (`_error` fica `null`), mas não marcava
  `IsAborted=true` — o resultado final vinha como conclusão bem-sucedida mesmo quando o handler
  "desistiu" via cancelamento. Corrigido com um novo campo `_cancelledByHandler` que força
  `IsAborted=true` nesse caso (mostrando "Canceled", não "Error!", já que continua não sendo erro).

**Achados confirmados por sonda, não são bugs** (mesma convenção já documentada pro `ProgressBar`):
cancelamento externo via `CancellationToken` (não Escape) sempre limpa a tela e pula
`FinishTemplate` inteiramente. **Correção sobre uma nota anterior desta mesma sessão** (investigação
mais profunda feita só ao chegar no `Time`, ver seção dele): esse cancelamento externo **não**
popula `result.Content` com um estado real "no momento do cancelamento" como eu tinha registrado
antes — na prática o `Content` volta como o valor `default` (ex.: `TimeSpan.Zero`/elapsed zerado),
porque o wake-up de tick que está em andamento no exato momento do cancelamento verifica o token
ANTES de voltar a ler uma tecla de verdade, então o branch `press.IsCancelled` de cada controle
raramente chega a rodar — o `ResultCtrl` acaba nulo e o fallback genérico do `BaseControlPrompt`
(`default!`) é quem populam o resultado. Só o abort via **Escape** (tecla real) garante estado real
populado, porque nesse caso o cancelamento nunca passa pelo `cts.Token` externo. Os testes deste
arquivo já refletiam isso corretamente (o teste de cancelamento externo só verifica
`IsAborted`+tela em branco, nunca um valor específico de `Content`) — só a prosa desta nota estava
imprecisa. `Finish(texto, textoerro)` — dois parâmetros, diferente do `ProgressBar` (só um) —
confirmado que o texto de erro é usado exclusivamente quando há exceção real, nunca no caminho de
abort simples (Escape/cancelamento).

Sample (`TaskControlSamples`) não precisou de nenhuma alteração — build confirmado limpo, nenhum
bug mudou API pública.

**Build validado + suíte completa revalidada em net10.0, estável em 2 execuções completas
seguidas**: 635 testes (PromptPlus.Tests).

### MultiTasks — concluído em 2026-07-23 (mesma sessão)

15 testes novos (`MultiTasksControlTests.cs`, já em `[Collection(BackgroundTimingCollection.Name)]`
desde o início). Mesma família de callback-do-chamador rodando numa `Task` de fundo, mas mais
complexo: N tarefas (`AddTask`/`AddTaskAsync`, 6 sobrecargas), execução `Sequential`/`Parallel` por
tarefa (grupos CONSECUTIVOS do mesmo modo rodam juntos, ordem da lista nunca muda),
`MaxDegreeOfParallelism`, `StopOnError` (só afeta sequencial), paginação da lista (reaproveita
`Paginator<T>`, mesmo padrão de MultiSelect). Uma falha em uma tarefa NUNCA conta como
`StateMultiTasks.Aborted=true` — isso é só pra Escape/cancelamento externo do RUN inteiro; falhas
por tarefa ficam em `Results`/`AnyFailed`/`AllSucceeded`. Confirmado por leitura que essa separação
já estava correta antes desta sessão (diferente do padrão de bug do ProgressBar/erro-vira-sucesso).

**1 bug real corrigido, a pedido do usuário depois de uma pergunta espontânea sobre o formato do
resumo**: `WriteSummary` (rodando) mostrava `"{done}/{total}"` (done = sucesso+falha) enquanto
`WriteFinishSummary` (frame final) mostrava `"{success}/{total}"` na MESMA posição da string — o
numerador visível mudava de significado silenciosamente no último frame (ex.: "2/2" rodando podia
virar "1/2 (1 failed)" no final, para as MESMAS 2 tarefas). Usuário perguntou se um formato com os
3 números explícitos ficaria mais claro; confirmou o formato e pediu tradução pras 11 culturas
mesmo sem revisão nativa. Implementado: `"{success} ok, {failed} failed, {waiting} wait"` — usado
IDENTICAMENTE por `WriteSummary` e `WriteFinishSummary` agora (nunca mais diverge). Recursos novos
`MultiTasksSuccessCount`/`MultiTasksWaitingCount` adicionados; `MultiTasksFailed` reaproveitado (só
removidos os parênteses/espaço fixos do valor) — todos os 11 `.resx` (de-DE, es-ES, fr-FR, it-IT,
ja-JP, ko-KR, nl-BE, pt-BR, ru-RU, zh-CN + neutro) atualizados, sem revisão nativa das traduções
novas (usuário ciente e aprovou essa condição).

**Achados confirmados por sonda, não são bugs**: `StopOnError` só existe no branch sequencial do
`ExecuteAllAsync` — confirmado por leitura (não por teste específico de regressão) que o branch
paralelo nunca checa essa flag, então uma falha numa tarefa `Parallel` nunca para as outras do
mesmo subconjunto, mesmo com `StopOnError()` ligado (documentado assim na doc XML do método, e o
teste `StopOnError_is_ignored_in_parallel_mode` trava esse comportamento). `MaxDegreeOfParallelism`
realmente limita a concorrência (confirmado via contador `Interlocked` em teste, não só leitura).
Cancelamento externo (não Escape) segue a mesma convenção do ProgressBar/TaskExec — tela em branco.
**Correção sobre uma nota anterior**: só o abort via **Escape** garante um snapshot real do estado
de cada tarefa no momento (o teste `Escape_aborts_and_captures_a_snapshot_of_current_task_states`
confirma isso, inclusive vendo uma tarefa ainda `Running` no instante do Escape); o cancelamento
EXTERNO via `CancellationToken` não tem essa garantia (mesma causa raiz encontrada na investigação
do `Time`, ver seção dele) — por isso o teste de cancelamento externo aqui só verifica
`IsAborted`+tela em branco, nunca um valor específico de `Content`.

Sample (`MultiTasksControlSamples`) não precisou de nenhuma alteração de API — build confirmado
limpo (a mudança de formato do resumo é só visual, não afeta a superfície pública).

**Build validado + suíte completa revalidada em net10.0**: 650 testes (PromptPlus.Tests).

### Time — concluído em 2026-07-23 (mesma sessão) — **Grupo 6 COMPLETO**

9 testes novos (`TimeControlTests.cs`, em `[Collection(BackgroundTimingCollection.Name)]`). Único
controle do grupo movido por um `Stopwatch` real puro (`WaitKeypress` checa
`_stopwatch.Elapsed >= _duration`), sem callback do chamador pra hook determinístico — usuário
confirmou que os testes não precisam de timing determinístico, só que o tempo decorrido seja
coerente com a `Duration` configurada (nunca comparar um `Sleep` do teste com um valor exato).
**Nenhum bug real encontrado.**

Achados confirmados por sonda (documentados no cabeçalho do arquivo de teste):
- Na conclusão normal, `ResultCtrl` retorna sempre a `Duration` CONFIGURADA (não o
  `_stopwatch.Elapsed` medido, que pode passar um pouco por causa do tick) — então a asserção de
  sucesso É exata, sem tolerância nenhuma necessária.
- `DisplayMode` só afeta o TEXTO renderizado (`Countdown` zera, `Elapsed` trava na `Duration`
  cheia) — o valor retornado (`Content`) nunca muda com o `DisplayMode`.
- `Duration` default é `TimeSpan.Zero` — sem chamar `.Duration(...)`, o controle termina
  imediatamente (não lança exceção de "config faltando", diferente de
  ProgressBar/TaskExec/MultiTasks).
- Mesma causa raiz encontrada aqui que motivou a correção das notas de TaskExec/MultiTasks acima:
  cancelamento externo (`CancellationToken`, não Escape) não garante `Content` com estado real —
  fica com o valor `default` (`TimeSpan.Zero`). Só Escape garante estado real (não passa pelo
  `cts.Token` externo).

Sample (`TimeControlSamples`) não precisou de nenhuma alteração — build confirmado limpo.

**Build validado + suíte completa revalidada em net10.0, estável em 2 execuções completas
seguidas**: 659 testes (PromptPlus.Tests). **Grupo 6 (Live) 100% concluído** — 4 controles, 61
testes novos, 7 bugs reais corrigidos no total (ProgressBar 3, TaskExec 3, MultiTasks 1, Time 0).

## Fora de escopo (decisões já fechadas, não são pendência)

- `AnsiDetector.cs`/`UnicodeDetector.cs` — decisão definitiva do usuário (2026-07-23): não serão cobertos.
- `Secret`/senha — **já concluído** (`InputSecretControlTests.cs`, fora deste plano por já estar feito).

## Progresso geral

- **Piloto (concluído antes deste plano)**: Select, Input — 5 arquivos de teste, ~64 testes, 1 bug real corrigido (#8).
- **Grupo 1 (4 controles)**: ✅ concluído — 53 testes novos, 2 bugs reais corrigidos no ChartBar (#9, #10) + 1 gap do driver de teste corrigido (SGR 256 cores).
- **Grupo 2 (5 controles)**: ✅ concluído — MultiSelect (39 testes, bug #11 + mudança de
  comportamento), Table (35 testes, bugs #12 e #13 + achado de UX do tooltip corrigido), Tree
  (32 testes, bugs #14 e #15 + conceito de nó desabilitado adicionado), MultiTable (35 testes,
  bugs #16 e #17, reaproveitando os 3 padrões já pré-aprovados sem reperguntar) e MultiTree
  (45 testes, bug #18 — `ComputeCheck`/`CollectCheckedFrom`/rodapé discordando após cascata +
  desmarcar individual — + conceito de nó desabilitado com semântica própria de cascata/tri-state).
- **Grupo 3 (1 controle)**: ✅ concluído — MaskEdit (73 testes, 4 bugs reais: `U[...]`/`{U[...]}`
  aceitando minúscula, tooltip Emacs incondicional, sinal negativo perdido em Integer/Long, 2
  typos de resource — + 1 melhoria de validação eager).
- **Grupo 4 (1 controle)**: ✅ concluído — Calendar (41 testes, nenhum bug real encontrado).
- **Grupo 5 (2 controles)**: ✅ concluído — FileControl (18 testes) + MultiFileControl (26 testes),
  nenhum bug real encontrado em nenhum dos dois (só a renomeação padrão `PredicateChecked` no
  MultiFile). Técnica nova de teste para controles com tarefas assíncronas de fundo documentada
  acima, reutilizável em Grupo 6.
- **Grupo 6 (Live, 4 controles)**: ✅ concluído — ProgressBar (22 testes, 3 bugs reais), TaskExec
  (15 testes, 3 bugs reais), MultiTasks (15 testes, 1 bug real — resumo final com métrica
  divergente do resumo rodando, resolvido com o formato explícito "ok/failed/wait"), Time (9
  testes, nenhum bug real — único movido por `Stopwatch` puro, sem hook do chamador). Estratégia
  por controle, não uma regra única (ver seção do grupo).

Total de testes após Grupo 6 (completo): 313 (ConsolePlus.Tests) + 659 (PromptPlus.Tests) =
**972 testes**, verde em net10.0 (ver [[feedback-test-validation-speed]] pra política de
validação, e a nota sobre `BackgroundTimingCollection` na seção do Grupo 6 pra por que a suíte
precisou de isolamento de paralelismo pra ficar estável). **Fase 2 (Grupos 1-6) 100% concluída.**
29 bugs reais de produção corrigidos no total ao longo de toda a Fase 2.

---

**Fim do escopo deste documento.** Continuação (pós-Fase 2, 2026-07-24) em
[`tests/TEST-PLAN.md`](TEST-PLAN.md), seção 11 ("Fase 3 — Resize"): auditoria da classe base
`BaseControlPrompt` (bugs #30 `ViewportSlice`, #31 cache estático de cultura em
Emacs-tooltips/aviso-de-resize, #32 cursor/scroll resetado por resize em Input/Select/MultiSelect/
Table/MultiTable/Tree) + extensão do driver de teste (`VirtualScreen.Resize` real, antes só
simulado). Total atualizado: 313 (ConsolePlus.Tests) + 667 (PromptPlus.Tests) = **980 testes**,
**32 bugs reais** no total.
