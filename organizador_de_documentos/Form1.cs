using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UglyToad.PdfPig;
using IronOcr;

namespace organizador_de_documentos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dgvResultados.Columns.Add("Arquivo", "Arquivo");
            dgvResultados.Columns.Add("Categoria", "Categoria");
            btnProcessar.Click += btnProcessar_Click;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void btnSelecionar_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = "Arquivos PDF|*.pdf";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in openFileDialog1.FileNames)
                {
                    dgvResultados.Rows.Add(file, "Não processado");
                }
            }
        }

        private void btnProcessar_Click(object sender, EventArgs e)
        {
            txtLog.Clear();

            foreach (DataGridViewRow row in dgvResultados.Rows)
            {
                string caminho = row.Cells[0].Value?.ToString();
                if (string.IsNullOrWhiteSpace(caminho)) continue;

                string texto = ExtrairTextoPdf(caminho);
                if (string.IsNullOrWhiteSpace(texto))
                {
                    texto = ExtrairTextoViaOcr(caminho);
                }

                string categoria = DetectarCategoria(texto);
                row.Cells[1].Value = categoria;
                txtLog.AppendText($"[{categoria}] {Path.GetFileName(caminho)}\r\n");
            }
        }
        private string ExtrairTextoPdf(string caminhoPdf)
        {
            try
            {
                using (var document = PdfDocument.Open(caminhoPdf))
                {
                    string textoCompleto = "";
                    foreach (var page in document.GetPages())
                    {
                        textoCompleto += page.Text + "\n";
                    }
                    return textoCompleto;
                }
            }
            catch
            {
                return "";
            }
        }

        private string ExtrairTextoViaOcr(string caminhoPdf)
        {
            try
            {
                var ocr = new IronTesseract();
                var input = new OcrInput();
                input.LoadPdf(caminhoPdf); 
                var result = ocr.Read(input);
                return result.Text;
            }
            catch (Exception ex)
            {
                return $"Erro ao executar OCR: {ex.Message}";
            }
        }


        private string DetectarCategoria(string texto)
        {
            texto = texto.ToLower();
            if (texto.Contains("boleto") || texto.Contains("vencimento"))
                return "Boleto";
            else if (texto.Contains("contrato") || texto.Contains("cláusula"))
                return "Contrato";
            else if (texto.Contains("certidão") || texto.Contains("registro"))
                return "Certidão";
            else
                return "Outros";
        }

        private void btnOrganizar_Click(object sender, EventArgs e)
        {
            string pastaDocumentos = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string pastaBase = Path.Combine(pastaDocumentos, "PDFsOrganizados");

            foreach (DataGridViewRow row in dgvResultados.Rows)
            {
                string caminho = row.Cells[0].Value?.ToString();
                string categoria = row.Cells[1].Value?.ToString();

                if (string.IsNullOrWhiteSpace(caminho) || string.IsNullOrWhiteSpace(categoria))
                    continue;

                try
                {
                    string destinoCategoria = Path.Combine(pastaBase, categoria);
                    Directory.CreateDirectory(destinoCategoria);

                    string nomeArquivo = Path.GetFileName(caminho);
                    string destinoFinal = Path.Combine(destinoCategoria, nomeArquivo);

                    // Verifica se o arquivo já existe
                    if (File.Exists(destinoFinal))
                    {
                        DialogResult resposta = MessageBox.Show(
                            $"O arquivo \"{nomeArquivo}\" já existe na categoria \"{categoria}\".\nDeseja substituir?",
                            "Arquivo já existe",
                            MessageBoxButtons.YesNoCancel,
                            MessageBoxIcon.Warning);

                        if (resposta == DialogResult.Cancel)
                            break;
                        else if (resposta == DialogResult.No)
                            continue; // pula este arquivo
                                      // se for Yes, continua e sobrescreve
                    }

                    File.Copy(caminho, destinoFinal, overwrite: true);
                    txtLog.AppendText($"✔ {nomeArquivo} copiado para {destinoCategoria}\r\n");
                }
                catch (Exception ex)
                {
                    txtLog.AppendText($"Erro ao copiar {caminho}: {ex.Message}\r\n");
                }
            }

            // Exibe mensagem e abre a pasta
            MessageBox.Show("Arquivos organizados com sucesso!", "Pronto", MessageBoxButtons.OK, MessageBoxIcon.Information);

            try
            {
                System.Diagnostics.Process.Start("explorer.exe", pastaBase);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível abrir a pasta: " + ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
