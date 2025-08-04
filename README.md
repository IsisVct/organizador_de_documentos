
# 📄 PDF Text Organizer com OCR – WinForms (.NET)

Este projeto é uma aplicação Windows Forms desenvolvida em C# que permite:

- ✅ Selecionar múltiplos arquivos PDF  
- ✅ Classificá-los manualmente por categoria  
- ✅ Extrair texto com OCR (mesmo de PDFs escaneados)  
- ✅ Organizar os arquivos em pastas por categoria dentro de **Meus Documentos**  
- ✅ Verificar duplicatas antes de copiar  
- ✅ Abrir automaticamente a pasta final após o processo

---

## 🛠️ Tecnologias e Bibliotecas

- [.NET Framework 4.8](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net48)
- [WinForms](https://learn.microsoft.com/dotnet/desktop/winforms/)
- [`PdfPig`](https://github.com/UglyToad/PdfPig) – Para leitura de PDFs com texto nativo
- [`IronOcr`](https://ironsoftware.com/csharp/ocr/) – Para OCR de PDFs digitalizados

---

## 📦 Funcionalidades principais

| Recurso                     | Descrição                                                                 |
|-----------------------------|---------------------------------------------------------------------------|
| Seleção de arquivos         | Abre um `OpenFileDialog` para escolher múltiplos PDFs                    |
| Extração de texto PDF       | Usa `PdfPig` para extrair texto nativo                                   |
| Extração via OCR            | Usa `IronOcr` para ler conteúdo de PDFs digitalizados                    |
| Organização de arquivos     | Copia os PDFs para `Documentos/PDFsOrganizados/{categoria}`             |
| Detecção de duplicatas      | Informa e pergunta se o usuário deseja substituir arquivos repetidos     |
| Abertura da pasta final     | Ao fim do processo, abre o Windows Explorer na pasta destino             |
| Log detalhado               | Mostra o status de cada arquivo no `TextBox`                             |

---

## 🖼️ Interface 

<img width="795" height="525" alt="image" src="https://github.com/user-attachments/assets/71f5ee4d-ccd4-4c35-8e37-b17a7cb320af" />

---

## ▶️ Como executar

1. Clone este repositório:
   ```bash
   git clone https://github.com//IsisVct/organizador_de_documentos.git
   ```

2. Abra o projeto no **Visual Studio** (versão 2022 ou superior)

3. Instale os pacotes NuGet necessários:
   - `UglyToad.PdfPig`
   - `IronOcr`

4. Compile e execute (`F5`)

---

## ⚙️ Estrutura do projeto

```
📁 PdfOrganizerOCR
│
├── Form1.cs             # Interface principal
├── ExtrairTextoPdf()    # Usa PdfPig
├── ExtrairTextoViaOcr() # Usa IronOcr com LoadPdf
└── README.md
```

---

## 🔐 Aviso de segurança

⚠️ **O pacote `SixLabors.ImageSharp 2.1.10`**, usado internamente por `IronOcr`, possui uma vulnerabilidade moderada.  
→ Recomenda-se **forçar a atualização para versão segura** (`3.0.2` ou superior) via NuGet.

---


## 🧑‍💻 Autor

**Isabelle Souza**  
💼 Projeto desenvolvido para fins de aprendizado em C# com foco em manipulação de arquivos PDF.
