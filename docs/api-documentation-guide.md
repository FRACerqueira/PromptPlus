<div align="center">
  <img src="../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Guia de Documentação da API**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../README.md) • **Next:** [KeyPress Control →](controls/keypress/index.md)

---

Este documento explica como a documentação da API do PromptPlus é gerada e mantida.

## 🛠️ Ferramenta Utilizada

A documentação da API é gerada automaticamente usando **[DefaultDocumentation](https://github.com/Doraku/DefaultDocumentation)** versão 1.2.5.

DefaultDocumentation é uma ferramenta que converte os comentários XML do código C# em arquivos Markdown, criando uma documentação navegável e completa da API.

## 📁 Estrutura da Documentação

```
docs/
├── api/                          # Documentação da API (gerada automaticamente)
│   ├── PromptPlus.md           # Página principal do assembly
│   ├── PromptPlusLibrary.*.md  # Páginas de tipos e membros
│   └── links.json               # Links externos (opcional)
├── getting-started.md           # Guias manuais
├── [others].md
└── ...
```

## ⚙️ Configuração

A configuração do DefaultDocumentation está no arquivo `src/PromptPlus.csproj`:

```xml
<PropertyGroup>
	<!-- Gera XML de documentação -->
	<GenerateDocumentationFile>True</GenerateDocumentationFile>
</PropertyGroup>

<!-- DefaultDocumentation APENAS em Release e no target net10.0 -->
<ItemGroup Condition="'$(Configuration)' == 'Release' and '$(TargetFramework)' == 'net10.0'">
	<PackageReference Include="DefaultDocumentation" Version="1.2.5">
		<PrivateAssets>all</PrivateAssets>
		<IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
	</PackageReference>
</ItemGroup>

<PropertyGroup Condition="'$(Configuration)' == 'Release' and '$(TargetFramework)' == 'net10.0'">
	<DefaultDocumentationFolder>..\docs\api</DefaultDocumentationFolder>
	<DefaultDocumentationGeneratedPages>Assembly, Namespaces , Types , Members</DefaultDocumentationGeneratedPages>
	<DefaultDocumentationGeneratedAccessModifiers>Public</DefaultDocumentationGeneratedAccessModifiers>
	<DefaultDocumentationAssemblyPageName>PromptPlus</DefaultDocumentationAssemblyPageName>
	<DocIconUrl>https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png</DocIconUrl>
	<DocIconWidth>120</DocIconWidth>
</PropertyGroup>
```

> ℹ️ Após a geração, uma task MSBuild (`PrependDocIconHeader`) adiciona o cabeçalho com o ícone
> (`DocIconUrl` / `DocIconWidth`) no topo de cada arquivo `.md` gerado em `docs/api`.

### Opções de Configuração

| Propriedade | Valor | Descrição |
|-------------|-------|-----------|
| `Condition` | `Release` + `net10.0` | **Documentação gerada APENAS em builds Release do target net10.0** |
| `DefaultDocumentationFolder` | `../docs/api` | Pasta de saída para os arquivos Markdown |
| `DefaultDocumentationGeneratedPages` | `Assembly, Namespaces, Types, Members` | Páginas geradas |
| `DefaultDocumentationGeneratedAccessModifiers` | `Public` | Documenta apenas membros públicos |
| `DefaultDocumentationAssemblyPageName` | `PromptPlus` | Nome da página principal do assembly (`PromptPlus.md`) |
| `DocIconUrl` / `DocIconWidth` | icon.png / `120` | Cabeçalho com ícone adicionado a cada `.md` gerado |

## 🔄 Regenerando a Documentação

A documentação é regenerada automaticamente toda vez que você compila o projeto em **Release**
para o target **net10.0**:

### Via Visual Studio
1. Abra a solução no Visual Studio
2. Mude para configuração **Release**
3. Build → Build Solution (Ctrl+Shift+B)
4. Os arquivos Markdown serão atualizados em `docs/api/`

**Nota**: Em builds **Debug** (ou em targets diferentes de net10.0), a documentação **não** é
gerada, para acelerar o desenvolvimento.

### Via Linha de Comando
```bash
# Na raiz do repositório - APENAS Release gera documentação (target net10.0)
dotnet build src/PromptPlus.csproj -c Release -f net10.0

# Build Debug NÃO gera documentação
dotnet build src/PromptPlus.csproj -c Debug
```

### Verificando os arquivos gerados
```powershell
# Verificar arquivos gerados
Get-ChildItem ..\docs\api\*.md | Select-Object Name, LastWriteTime
```

## ✍️ Escrevendo Comentários XML

Para que a documentação seja gerada corretamente, adicione comentários XML no código:

```csharp
/// <summary>
/// Escreve o texto especificado no console.
/// </summary>
/// <param name="text">O texto a ser escrito.</param>
/// <remarks>
/// Este método suporta markup inline para estilização.
/// Exemplo: <c>[red]Texto vermelho[/]</c>
/// </remarks>
/// <example>
/// <code>
/// PromptPlus.Console.WriteLine("Hello, [blue]World[/]!");
/// </code>
/// </example>
public static void WriteLine(string text)
{
	// implementação
}
```

### Tags XML Suportadas

| Tag | Uso |
|-----|-----|
| `<summary>` | Descrição breve do membro |
| `<param>` | Descrição de parâmetros |
| `<returns>` | Descrição do valor de retorno |
| `<remarks>` | Informações adicionais |
| `<example>` | Exemplos de uso |
| `<code>` | Blocos de código |
| `<see>` | Referências cruzadas |
| `<seealso>` | Ver também |
| `<exception>` | Exceções que podem ser lançadas |

## 📝 Boas Práticas

1. **Seja Conciso**: Mantenha o `<summary>` em uma linha
2. **Documente Tudo Público**: Todos os membros públicos devem ter documentação
3. **Use Exemplos**: Adicione `<example>` para métodos complexos
4. **Referencie Outros Tipos**: Use `<see cref="ClassName"/>` para criar links
5. **Documente Exceções**: Use `<exception>` para documentar erros possíveis
6. **Markdown nos Comentários**: Você pode usar markdown dentro de `<remarks>`

## 🔍 Verificando a Qualidade

### Avisos de Documentação

Para garantir que toda a API pública está documentada, você pode habilitar avisos:

```xml
<PropertyGroup>
	<!-- Avisos para membros públicos sem documentação -->
	<GenerateDocumentationFile>True</GenerateDocumentationFile>
	<NoWarn>$(NoWarn);CS1591</NoWarn> <!-- Remover para ver avisos -->
</PropertyGroup>
```

Remova `;CS1591` de `NoWarn` para ver avisos de documentação faltando.

## 🌐 Links Externos (Opcional)

O arquivo `docs/api/links.json` permite configurar links externos para tipos do .NET Framework:

```json
{
  "System": "https://learn.microsoft.com/en-us/dotnet/api/system",
  "System.Console": "https://learn.microsoft.com/en-us/dotnet/api/system.console",
  "System.Threading.Tasks": "https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks"
}
```

Adicione novos tipos conforme necessário para melhorar os links da documentação.

## 🚀 Publicando a Documentação

### GitHub Pages

Para publicar a documentação no GitHub Pages:

1. Configure o repositório para usar GitHub Pages
2. Aponte para a branch/pasta que contém os arquivos Markdown
3. A documentação estará disponível em `https://username.github.io/PromptPlus/`

### ReadTheDocs ou Outras Plataformas

Os arquivos Markdown gerados podem ser usados com qualquer plataforma de documentação que suporte Markdown.

## 🐛 Solução de Problemas

### Documentação Não Está Sendo Gerada

1. Verifique se `GenerateDocumentationFile` está `True`
2. Confirme que o pacote DefaultDocumentation está instalado
3. Faça um Clean + Rebuild da solução
4. Verifique erros de build no Output

### Avisos de Build

Se ver avisos relacionados ao DefaultDocumentation, verifique:
- A versão do pacote é compatível com seu .NET SDK
- Todas as propriedades de configuração estão corretas
- Não há conflitos com outros analisadores

### Links Quebrados

Se houver links quebrados na documentação:
- Verifique se os namespaces/tipos referenciados existem
- Atualize `links.json` para tipos externos
- Use `<see cref="">` corretamente nos comentários XML

## 📦 Controle de Versão

### O que Commitar

✅ **Commitar**:
- Arquivos de configuração (`PromptPlus.csproj`)
- Comentários XML no código-fonte
- `links.json` (se usado)

❓ **Opcional**:
- Arquivos `.md` gerados em `docs/api/`
  - **Commitar**: Para ter histórico e facilitar revisão em PRs
  - **Não commitar**: Se preferir gerar sob demanda (adicionar `docs/api/*.md` ao `.gitignore`)

A escolha depende da preferência da equipe. Commitar permite ver mudanças na documentação em PRs.

## 🤝 Contribuindo

Ao fazer um Pull Request que adiciona ou modifica API pública:

1. ✅ Adicione comentários XML completos
2. ✅ Inclua exemplos quando apropriado
3. ✅ Regenere a documentação (build)
4. ✅ Verifique se os arquivos `.md` foram atualizados
5. ✅ Revise a documentação gerada para qualidade

## 📚 Recursos

- [DefaultDocumentation no GitHub](https://github.com/Doraku/DefaultDocumentation)
- [XML Documentation Comments (Microsoft)](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/)
- [Recommended XML Tags (Microsoft)](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags)

---

**Última atualização**: Este guia foi criado junto com a configuração inicial do DefaultDocumentation para o PromptPlus.
