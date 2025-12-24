# Guia para Criar o Instalador do Genuss Automação

## ✅ Status Atual

O projeto foi compilado com sucesso! Os arquivos estão prontos para criar o instalador.

### Estatísticas da Compilação:
- **Executáveis**: 2 arquivos
- **Bibliotecas (DLL)**: 34 arquivos
- **Configurações**: 3 arquivos
- **Arquivos XML**: 12 arquivos
- **Tamanho Total**: 168.17 MB

## 📋 Próximos Passos

### 1. Instalar o Inno Setup

1. Baixe o Inno Setup 6.0 ou superior em: https://jrsoftware.org/isinfo.php
2. Execute o instalador e siga as instruções padrão
3. Certifique-se de instalar com as opções padrão

### 2. Compilar o Instalador

1. **Abra o Inno Setup Compiler**
2. **Abra o arquivo**: `Setup_Genuss_Automacao.iss` (localizado na pasta raiz do projeto)
3. **Clique em**: `Build` → `Compile` (ou pressione F9)
4. **Aguarde** a compilação (pode levar alguns minutos)
5. **O instalador será criado** na pasta: `F:\pdv\pdv\Controle de Vendas\Setup_Output`

### 3. Arquivos Criados

Após a compilação, você terá:
- ✅ **Setup_Genuss_Automacao.iss** - Script do Inno Setup
- ✅ **LICENSE.txt** - Termos de licença
- ✅ **README_INSTALACAO.txt** - Guia de instalação
- ✅ **PrepararSetup.ps1** - Script de preparação
- 🔄 **Setup_Genuss_Automacao_v1.0.0.exe** - Instalador final (será criado)

## 🎯 Características do Instalador

### Funcionalidades Incluídas:
- ✅ Verificação automática de pré-requisitos (.NET Framework 4.8)
- ✅ Verificação do Access Database Engine
- ✅ Instalação de todas as dependências
- ✅ Criação de atalhos no desktop e menu iniciar
- ✅ Registro de associações de arquivo
- ✅ Criação de diretórios necessários
- ✅ Interface em português brasileiro
- ✅ Desinstalador automático

### Pré-requisitos Verificados:
- Microsoft .NET Framework 4.8 ou superior
- Microsoft Access Database Engine 2016 Redistributable
- Windows 10 ou superior (64-bit)

## 🔧 Dependências Incluídas

### Bibliotecas Principais:
- ✅ **Aspose.PDF** - Manipulação de PDF
- ✅ **Newtonsoft.Json** - Processamento JSON
- ✅ **QRCoder** - Geração de QR Code
- ✅ **System.Data.SQLite** - Banco de dados SQLite
- ⚠️ **ACBrLib.Core** - Automação Comercial (verificar se está na pasta bin)
- ⚠️ **Bunifu.UI.WinForms** - Interface moderna (verificar se está na pasta bin)
- ⚠️ **ZeusNFe** - Emissão de NFCe (verificar se está na pasta bin)

### Nota sobre Dependências Ausentes:
Algumas DLLs importantes não foram encontradas na pasta bin/Release. Isso pode acontecer se:
1. As bibliotecas estão em uma pasta diferente
2. Precisam ser copiadas manualmente
3. Requerem instalação separada

## 🚀 Testando o Instalador

### Antes de Distribuir:
1. **Teste em uma máquina limpa** (sem o Visual Studio)
2. **Verifique se todos os recursos funcionam**:
   - Abertura do sistema
   - Conexão com banco de dados
   - Emissão de NFCe (se configurado)
   - Impressão de cupons
3. **Teste a desinstalação**
4. **Verifique se não há erros de DLL faltando**

### Solução para DLLs Ausentes:
Se encontrar erros de DLL faltando:
1. Copie as DLLs necessárias para a pasta `bin\Release`
2. Execute novamente o script `PrepararSetup.ps1`
3. Recompile o instalador no Inno Setup

## 📁 Estrutura de Arquivos

```
F:\pdv\pdv\Controle de Vendas\
├── Setup_Genuss_Automacao.iss     # Script do instalador
├── LICENSE.txt                     # Licença
├── README_INSTALACAO.txt          # Guia de instalação
├── PrepararSetup.ps1              # Script de preparação
├── Setup_Output\                  # Pasta do instalador final
└── Controle de Vendas\bin\Release\ # Arquivos compilados
```

## 🎉 Finalização

Após seguir todos os passos, você terá um instalador profissional do Genuss Automação que:
- Instala automaticamente todas as dependências
- Verifica pré-requisitos do sistema
- Cria atalhos e associações de arquivo
- Inclui documentação e licença
- Permite desinstalação limpa

---

**Criado em**: Janeiro 2024  
**Versão**: 1.0.0  
**Sistema**: Genuss Automação - Sistema de Vendas