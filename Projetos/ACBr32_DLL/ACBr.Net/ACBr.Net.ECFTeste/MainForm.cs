﻿using System;
using System.IO;
using System.Windows.Forms;

namespace ACBr.Net.ECFTeste
{
    public partial class MainForm : Form
    {
        #region Fields

        private ACBrECF acbrECF;
        private ACBrAAC acbrAAC;

        #endregion Fields

        #region Constructor

        public MainForm()
        {
            InitializeComponent();

            this.acbrECF = new ACBrECF();
            this.acbrAAC = new ACBrAAC();

            Popular();
            PopularAAC();
        }

        #endregion Constructor

        #region Methods

        #region ACC

        private void PopularAAC()
        {
            aacTipoDesenvolvimentoComboBox.Items.Clear();
            foreach (var desenv in Enum.GetValues(typeof(ACBrPAFTipoDesenvolvimento))) aacTipoDesenvolvimentoComboBox.Items.Add(desenv);

            aacTipoIntegracaoComboBox.Items.Clear();
            foreach (var integ in Enum.GetValues(typeof(ACBrPAFTipoIntegracao))) aacTipoIntegracaoComboBox.Items.Add(integ);

            aacTipoFuncionamentoComboBox.Items.Clear();
            foreach (var func in Enum.GetValues(typeof(ACBrPAFTipoFuncionamento))) aacTipoFuncionamentoComboBox.Items.Add(func);
        }

        private void CarregarAAC()
        {
            acbrAAC.Chave = aacChaveTextBox.Text;
            acbrAAC.NomeArquivoAuxiliar = aacNomeArquivoTextbox.Text;
            try
            {
                acbrAAC.AbrirArquivo();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            //Dados da software house
            aacRazaoSocialTextBox.Text = acbrAAC.IdentPaf.Empresa.RazaoSocial;
			aacIETextBox.Text = acbrAAC.IdentPaf.Empresa.IE;
			aacIMTextBox.Text = acbrAAC.IdentPaf.Empresa.IM;
			aacCNPJTextBox.Text = acbrAAC.IdentPaf.Empresa.CNPJ;

            //Dados do aplicativo
            aacBancoDadosTextBox.Text = acbrAAC.IdentPaf.Paf.BancoDeDadosAplicativo;
			aacLinguagemTextBox.Text = acbrAAC.IdentPaf.Paf.LinguagemAplicativo;
			aacMD5TextBox.Text = acbrAAC.IdentPaf.Paf.MD5Aplicativo;
			aacNomePAFTextBox.Text = acbrAAC.IdentPaf.Paf.NomeAplicativo;
			aacPrincipalExeTextBox.Text = acbrAAC.IdentPaf.Paf.PrincipalExeAplicativo;
			aacSOTextBox.Text = acbrAAC.IdentPaf.Paf.SistemaOperacionalAplicativo;
			aacVersaoTextBox.Text = acbrAAC.IdentPaf.Paf.VersaoAplicativo;

            //Dados de funcionalidade
			aacTipoDesenvolvimentoComboBox.SelectedItem = acbrAAC.IdentPaf.Paf.TipoDesenvolvimento;
			aacTipoFuncionamentoComboBox.SelectedItem = acbrAAC.IdentPaf.Paf.TipoFuncionamento;
			aacTipoIntegracaoComboBox.SelectedItem = acbrAAC.IdentPaf.Paf.TipoIntegracao;

            //Dados de não concomitância
			aacRealizaDavAnexoIICheckBox.Checked = acbrAAC.IdentPaf.Paf.RealizaDAVConfAnexoII;
			aacRealizaDavECFCheckBox.Checked = acbrAAC.IdentPaf.Paf.RealizaDAVECF;
			aacRealizaDavNaoFiscalCheckBox.Checked = acbrAAC.IdentPaf.Paf.RealizaDAVNaoFiscal;
			aacRealizaDavOSCheckBox.Checked = acbrAAC.IdentPaf.Paf.RealizaDAVOS;
			aacRealizaLancMesaCheckBox.Checked = acbrAAC.IdentPaf.Paf.RealizaLancamentoMesa;
			aacRealizaPreVendaCheckBox.Checked = acbrAAC.IdentPaf.Paf.RealizaPreVenda;

            //Dados de aplicações especiais
			aacImpressoraNaoFiscalProdCheckBox.Checked = acbrAAC.IdentPaf.Paf.UsaImpressoraNaoFiscal;
			aacIndTecProdCheckBox.Checked = acbrAAC.IdentPaf.Paf.IndiceTecnicoProducao;
			aacBalancaCheckBox.Checked = acbrAAC.IdentPaf.Paf.BarSimiliarBalanca;
			aacBarECFComumCheckBox.Checked = acbrAAC.IdentPaf.Paf.BarSimiliarECFComum;
			aacBarECFRestauranteCheckBox.Checked = acbrAAC.IdentPaf.Paf.BarSimiliarECFRestaurante;
			aacImprimeDAVDiscFormCheckBox.Checked = acbrAAC.IdentPaf.Paf.DAVDiscrFormula;

            //Dados critério por uf

			aacTotalizaValoresListaCheckBox.Checked = acbrAAC.IdentPaf.Paf.TotalizaValoresLista;
			aacListaPreVendaCheckBox.Checked = acbrAAC.IdentPaf.Paf.TransPreVenda;
			aacListaDAVCheckBox.Checked = acbrAAC.IdentPaf.Paf.TransDAV;
			aacRecompoeGTCheckBox.Checked = acbrAAC.IdentPaf.Paf.RecompoeGT;
			aacEmissaoDocFisPED.Checked = acbrAAC.IdentPaf.Paf.EmitePED;
			aacCupomManiaCheckBox.Checked = acbrAAC.IdentPaf.Paf.CupomMania;
			aacMinasLegalCheckBox.Checked = acbrAAC.IdentPaf.Paf.MinasLegal;

            //Dados parametros
            aacParamsTextBox.Text = acbrAAC.Parametros;

            //Dados ECFs autorizadas
            int numeroEcfs = acbrAAC.IdentPaf.ECFsAutorizados.Count;

            aacECFsAutorizadaDataGridView.Rows.Clear();

			foreach (ACBrAACECF ecfAutorizado in acbrAAC.IdentPaf.ECFsAutorizados)
            {
                aacECFsAutorizadaDataGridView.Rows.Add(ecfAutorizado.NumeroSerie, ecfAutorizado.CRO, ecfAutorizado.CNI, ecfAutorizado.ValorGT);
            }
        }

        private bool ValidarAAC()
        {
            string nomeArquivo = aacNomeArquivoTextbox.Text;
            string diretorio = Path.GetDirectoryName(nomeArquivo);
            bool validado = true;

            if (!Directory.Exists(diretorio))
            {
                validado = false;
                MessageBox.Show("Caminho especificado não é valido");
            }

            return validado;
        }

        private void RecriarAAC()
        {
            if (!ValidarAAC()) return;

            File.WriteAllText(aacNomeArquivoTextbox.Text, "");

            acbrAAC.Chave = aacChaveTextBox.Text;
            acbrAAC.NomeArquivoAuxiliar = aacNomeArquivoTextbox.Text;

            //Dados da software house
            acbrAAC.IdentPaf.Empresa.RazaoSocial = aacRazaoSocialTextBox.Text;
			acbrAAC.IdentPaf.Empresa.IE = aacIETextBox.Text;
			acbrAAC.IdentPaf.Empresa.IM = aacIMTextBox.Text;
			acbrAAC.IdentPaf.Empresa.CNPJ = aacCNPJTextBox.Text;
            //Dados do aplicativo
			acbrAAC.IdentPaf.Paf.BancoDeDadosAplicativo = aacBancoDadosTextBox.Text;
			acbrAAC.IdentPaf.Paf.LinguagemAplicativo = aacLinguagemTextBox.Text;
			acbrAAC.IdentPaf.Paf.MD5Aplicativo = aacMD5TextBox.Text;
			acbrAAC.IdentPaf.Paf.NomeAplicativo = aacNomePAFTextBox.Text;
			acbrAAC.IdentPaf.Paf.PrincipalExeAplicativo = aacPrincipalExeTextBox.Text;
			acbrAAC.IdentPaf.Paf.SistemaOperacionalAplicativo = aacSOTextBox.Text;
			acbrAAC.IdentPaf.Paf.VersaoAplicativo = aacVersaoTextBox.Text;
            //Dados de funcionalidade
            if (aacTipoDesenvolvimentoComboBox.SelectedItem != null)
            {
                acbrAAC.IdentPaf.Paf.TipoDesenvolvimento = (ACBrPAFTipoDesenvolvimento)aacTipoDesenvolvimentoComboBox.SelectedItem;
            }
            if (aacTipoFuncionamentoComboBox.SelectedItem != null)
            {
				acbrAAC.IdentPaf.Paf.TipoFuncionamento = (ACBrPAFTipoFuncionamento)aacTipoFuncionamentoComboBox.SelectedItem;
            }
            if (aacTipoIntegracaoComboBox.SelectedItem != null)
            {
				acbrAAC.IdentPaf.Paf.TipoIntegracao = (ACBrPAFTipoIntegracao)aacTipoIntegracaoComboBox.SelectedItem;
            }
            //Dados de não concomitância
			acbrAAC.IdentPaf.Paf.RealizaDAVConfAnexoII = aacRealizaDavAnexoIICheckBox.Checked;
			acbrAAC.IdentPaf.Paf.RealizaDAVECF = aacRealizaDavECFCheckBox.Checked;
			acbrAAC.IdentPaf.Paf.RealizaDAVNaoFiscal = aacRealizaDavNaoFiscalCheckBox.Checked;
			acbrAAC.IdentPaf.Paf.RealizaDAVOS = aacRealizaDavOSCheckBox.Checked;
			acbrAAC.IdentPaf.Paf.RealizaLancamentoMesa = aacRealizaLancMesaCheckBox.Checked;
			acbrAAC.IdentPaf.Paf.RealizaPreVenda = aacRealizaPreVendaCheckBox.Checked;

            //Dados de aplicações especiais
			acbrAAC.IdentPaf.Paf.UsaImpressoraNaoFiscal = aacImpressoraNaoFiscalProdCheckBox.Checked;
			acbrAAC.IdentPaf.Paf.IndiceTecnicoProducao = aacIndTecProdCheckBox.Checked;
			acbrAAC.IdentPaf.Paf.BarSimiliarBalanca = aacBalancaCheckBox.Checked;
			acbrAAC.IdentPaf.Paf.BarSimiliarECFComum = aacBarECFComumCheckBox.Checked;
			acbrAAC.IdentPaf.Paf.BarSimiliarECFRestaurante = aacBarECFRestauranteCheckBox.Checked;
			acbrAAC.IdentPaf.Paf.DAVDiscrFormula = aacImprimeDAVDiscFormCheckBox.Checked;

            //Dados critério por uf

			acbrAAC.IdentPaf.Paf.TotalizaValoresLista = aacTotalizaValoresListaCheckBox.Checked;
			acbrAAC.IdentPaf.Paf.TransPreVenda = aacListaPreVendaCheckBox.Checked;
			acbrAAC.IdentPaf.Paf.TransDAV = aacListaDAVCheckBox.Checked;
			acbrAAC.IdentPaf.Paf.RecompoeGT = aacRecompoeGTCheckBox.Checked;
			acbrAAC.IdentPaf.Paf.EmitePED = aacEmissaoDocFisPED.Checked;
			acbrAAC.IdentPaf.Paf.CupomMania = aacCupomManiaCheckBox.Checked;
			acbrAAC.IdentPaf.Paf.MinasLegal = aacMinasLegalCheckBox.Checked;

            //Dados parametros
            acbrAAC.Parametros = aacParamsTextBox.Text;

            //Dados ECFs autorizadas
            //ACBrDll.TECFAutorizado ecfAutorizado;

			acbrAAC.IdentPaf.ECFsAutorizados.Clear();

            foreach (DataGridViewRow row in aacECFsAutorizadaDataGridView.Rows)
            {
				ACBrAACECF ecfAutorizado = new ACBrAACECF();

                ecfAutorizado.ValorGT = Convert.ToDouble(row.Cells["valorGTColumn"].Value);
                ecfAutorizado.NumeroSerie = row.Cells["numSerieColumn"].Value.ToString();
                ecfAutorizado.CRO = Convert.ToInt32(row.Cells["CROColumn"].Value);
                ecfAutorizado.CNI = Convert.ToInt32(row.Cells["CNIColumn"].Value);
                
				acbrAAC.IdentPaf.ECFsAutorizados.New(ecfAutorizado);
            }

            try
            {
                acbrAAC.SalvarArquivo();
                MessageBox.Show("Arquivo de configuração criado com sucesso!");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        #endregion ACC

        #region ECF

        private void Popular()
        {
            modeloComboBox.Items.Clear();
            foreach (var modelo in Enum.GetValues(typeof(ModeloECF))) modeloComboBox.Items.Add(modelo);
            modeloComboBox.SelectedIndex = 0;

            portaComboBox.Items.Clear();
            portaComboBox.Items.Add("COM1");
            portaComboBox.Items.Add("COM2");
            portaComboBox.Items.Add("COM3");
            portaComboBox.Items.Add("COM4");
            portaComboBox.Items.Add("COM5");
            portaComboBox.Items.Add("COM6");
            portaComboBox.Items.Add("LPT1");
            portaComboBox.Items.Add("LPT2");
            portaComboBox.Items.Add("LPT3");
            portaComboBox.Items.Add("ecf.txt");
            portaComboBox.SelectedIndex = 0;

            velocidadeComboBox.Items.Add(1200);
            velocidadeComboBox.Items.Add(2400);
            velocidadeComboBox.Items.Add(4800);
            velocidadeComboBox.Items.Add(9600);
            velocidadeComboBox.Items.Add(19200);
            velocidadeComboBox.Items.Add(38400);
            velocidadeComboBox.Items.Add(57600);
            velocidadeComboBox.Items.Add(115200);
            velocidadeComboBox.SelectedItem = 9600;

            ativarCheckButton.Checked = false;
        }

        public void Ativar()
        {
            try
            {
                acbrECF.Modelo = (ModeloECF)modeloComboBox.SelectedItem;
                acbrECF.Porta = (string)portaComboBox.SelectedItem;
                acbrECF.Baud = (int)velocidadeComboBox.SelectedItem;
                acbrECF.TimeOut = (int)timeOutNumericUpDown.Value;

                acbrECF.GavetaSinalInvertido = gavetaCheckBox.Checked;
                acbrECF.DescricaoGrande = descricaoCheckBox.Checked;

                acbrECF.Ativar();
                ativarCheckButton.Checked = true;
                ativarCheckButton.Text = "Desativar";

                messageToolStripStatusLabel.Text = acbrECF.Estado.ToString();
                descriptionToolStripStatusLabel.Text = string.Empty;

                tabControl.SelectedTab = cmdTabPage;
                WriteResp("Ativado: OK!");
            }
            catch (Exception exception)
            {
                ativarCheckButton.Checked = false;

                messageToolStripStatusLabel.Text = "Exception";
                descriptionToolStripStatusLabel.Text = exception.Message;
            }
        }

        private void WriteResp(string resp)
        {
            if (string.IsNullOrEmpty(resp)) return;

            foreach (string line in resp.Split('\n'))
            {
                respListBox.Items.Add(line);
            }
            respListBox.Items.Add("+-+-+-+-+-+-+-+-+-+-+-+-+-+-");
            respListBox.Items.Add("");
            respListBox.SelectedIndex = respListBox.Items.Count - 1;
        }

        public void Desativar()
        {
            try
            {
                acbrECF.Desativar();
                messageToolStripStatusLabel.Text = "OK";
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (NullReferenceException)
            {
                messageToolStripStatusLabel.Text = "Não inicializado.";
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (Exception exception)
            {
                messageToolStripStatusLabel.Text = "Exception";
                descriptionToolStripStatusLabel.Text = exception.Message;
            }
        }

        public void Testar()
        {
            try
            {
                string message = string.Empty;

                message += string.Format("Impressora: {0}", acbrECF.ModeloStr);
                message += string.Format("\nVersão: {0}", acbrECF.NumVersao);
                message += string.Format("\nColunas: {0}", acbrECF.Colunas);
                message += string.Format("\n");
                message += string.Format("\nNúmero de série: {0}", acbrECF.NumSerie);
                message += string.Format("\nNum do ECF: {0}", acbrECF.NumECF);
                message += string.Format("\nData/Hora: {0:dd/MM/yyyy HH:mm:ss}", acbrECF.DataHora);

                messageToolStripStatusLabel.Text = acbrECF.Estado.ToString();
                descriptionToolStripStatusLabel.Text = string.Empty;

                MessageBox.Show(this, message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (NullReferenceException)
            {
                messageToolStripStatusLabel.Text = "Não inicializado.";
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (Exception exception)
            {
                messageToolStripStatusLabel.Text = "Exception";
                descriptionToolStripStatusLabel.Text = exception.Message;
            }
        }

        public void Sair()
        {
            this.Close();
        }

        public void Ler_Estado()
        {
            try
            {
                WriteResp(string.Format("\nEstado: {0}", acbrECF.Estado));
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (NullReferenceException)
            {
                messageToolStripStatusLabel.Text = "Não inicializado.";
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (Exception exception)
            {
                messageToolStripStatusLabel.Text = "Exception";
                descriptionToolStripStatusLabel.Text = exception.Message;
            }
        }

        public void Ler_DataHora()
        {
            try
            {
                WriteResp(string.Format("\nDataHora: {0}", acbrECF.DataHora));
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (NullReferenceException)
            {
                messageToolStripStatusLabel.Text = "Não inicializado.";
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (Exception exception)
            {
                messageToolStripStatusLabel.Text = "Exception";
                descriptionToolStripStatusLabel.Text = exception.Message;
            }
        }

        public void Ler_NumECF()
        {
            try
            {
                WriteResp(string.Format("\nNum ECF: {0}", acbrECF.NumECF));
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (NullReferenceException)
            {
                messageToolStripStatusLabel.Text = "Não inicializado.";
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (Exception exception)
            {
                messageToolStripStatusLabel.Text = "Exception";
                descriptionToolStripStatusLabel.Text = exception.Message;
            }
        }

        private void Ler_NumLoja()
        {
            try
            {
                WriteResp(string.Format("Num Loja: {0}", acbrECF.NumLoja));
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (NullReferenceException)
            {
                messageToolStripStatusLabel.Text = "Não inicializado.";
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (Exception exception)
            {
                messageToolStripStatusLabel.Text = "Exception";
                descriptionToolStripStatusLabel.Text = exception.Message;
            }
        }

        private void Ler_NumSerie()
        {
            try
            {
                WriteResp(string.Format("Num Serie: {0}", acbrECF.NumSerie));
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (NullReferenceException)
            {
                messageToolStripStatusLabel.Text = "Não inicializado.";
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (Exception exception)
            {
                messageToolStripStatusLabel.Text = "Exception";
                descriptionToolStripStatusLabel.Text = exception.Message;
            }
        }

        private void Ler_NumVersao()
        {
            try
            {
                WriteResp(string.Format("Num Versao: {0}", acbrECF.NumVersao));
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (NullReferenceException)
            {
                messageToolStripStatusLabel.Text = "Não inicializado.";
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (Exception exception)
            {
                messageToolStripStatusLabel.Text = "Exception";
                descriptionToolStripStatusLabel.Text = exception.Message;
            }
        }

        private void Ler_CNPJ()
        {
            try
            {
                WriteResp(string.Format("CNPJ: {0}", acbrECF.CNPJ));
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (NullReferenceException)
            {
                messageToolStripStatusLabel.Text = "Não inicializado.";
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (Exception exception)
            {
                messageToolStripStatusLabel.Text = "Exception";
                descriptionToolStripStatusLabel.Text = exception.Message;
            }
        }

        private void Ler_IE()
        {
            try
            {
                WriteResp(string.Format("IE: {0}", acbrECF.IE));
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (NullReferenceException)
            {
                messageToolStripStatusLabel.Text = "Não inicializado.";
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (Exception exception)
            {
                messageToolStripStatusLabel.Text = "Exception";
                descriptionToolStripStatusLabel.Text = exception.Message;
            }
        }

        private void Ler_PAF()
        {
            try
            {
                WriteResp(string.Format("PAF: {0}", acbrECF.PAF));
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (NullReferenceException)
            {
                messageToolStripStatusLabel.Text = "Não inicializado.";
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (Exception exception)
            {
                messageToolStripStatusLabel.Text = "Exception";
                descriptionToolStripStatusLabel.Text = exception.Message;
            }
        }

        private void Ler_DadosReducaoZ()
        {
            try
            {
                WriteResp(string.Format("DadosReducaoZ:\n{0}", acbrECF.DadosReducaoZ));
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (NullReferenceException)
            {
                messageToolStripStatusLabel.Text = "Não inicializado.";
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (Exception exception)
            {
                messageToolStripStatusLabel.Text = "Exception";
                descriptionToolStripStatusLabel.Text = exception.Message;
            }
        }

        private void Ler_DadosUltimaReducaoZ()
        {
            try
            {
                WriteResp(string.Format("DadosUltimaReducaoZ:\n{0}", acbrECF.DadosUltimaReducaoZ));
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (NullReferenceException)
            {
                messageToolStripStatusLabel.Text = "Não inicializado.";
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (Exception exception)
            {
                messageToolStripStatusLabel.Text = "Exception";
                descriptionToolStripStatusLabel.Text = exception.Message;
            }
        }

        private void Ler_TodasVariaveis()
        {
            var type = typeof(ACBrECF);
            foreach (var property in type.GetProperties())
            {
                try
                {
                    object value = property.GetValue(acbrECF, null);

                    if (value is System.Collections.ICollection)
                    {
                        var collection = (System.Collections.ICollection)value;
                        respListBox.Items.Add(string.Format("{0}:", property.Name));

                        foreach (var element in collection)
                        {
                            if (element == null) continue;

                            respListBox.Items.Add("");

                            Type elementType = element.GetType();

                            foreach (var elementProperty in elementType.GetProperties())
                            {
                                object elementValue = elementProperty.GetValue(element, null);
                                respListBox.Items.Add(string.Format("{0}: {1}", elementProperty.Name, elementValue));
                            }
                        }

                        respListBox.Items.Add("");
                        WriteResp(string.Format("{0} elemento(s)", collection.Count));
                    }
                    else
                    {
                        WriteResp(string.Format("{0}: {1}", property.Name, value));
                    }

                    descriptionToolStripStatusLabel.Text = string.Empty;
                }
                catch (NullReferenceException)
                {
                    messageToolStripStatusLabel.Text = "Não inicializado.";
                    descriptionToolStripStatusLabel.Text = string.Empty;
                }
                catch (Exception exception)
                {
                    messageToolStripStatusLabel.Text = "Exception";
                    descriptionToolStripStatusLabel.Text = exception.Message;
                }

                Application.DoEvents();
            }
        }

        private void TestaCupomFiscal()
        {
            try
            {
                if (acbrECF.Estado == EstadoECF.Livre)
                {
                    respListBox.Items.Add("Abrindo cupom ...");
                    acbrECF.AbreCupom("", "", "");
                    Application.DoEvents();
                }

                if (acbrECF.Estado == EstadoECF.Venda)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        respListBox.Items.Add("Vende Item #" + i + " ...");
                        acbrECF.VendeItem(string.Format("{0:0000000000000}", i + 1), "PRODUTO àáèéìíòóùúü " + i, "II", 1, 1.99M, 0M, "UN", "%", "D");
                        Application.DoEvents();
                    }

                    acbrECF.SubtotalizaCupom(0M, "Mensagem SubtotalizaCupom ACBr.NET");
                    respListBox.Items.Add("Subtotaliza Cupom ...");
                    Application.DoEvents();
                }

                if (acbrECF.Estado == EstadoECF.Pagamento)
                {
                    if (acbrECF.TotalPago == 0)
                    {
                        var forma01 = acbrECF.FormasPagamento[0];
                        acbrECF.EfetuaPagamento(forma01.Indice, 50M, "Mensagem EfetuaPagamento ACBr.NET", false);
                        respListBox.Items.Add("Efetua Pagamento ...");
                        Application.DoEvents();
                    }

                    acbrECF.FechaCupom("Mensagem àáèéìíòóùúü FechaCupom ACBr.NET");
                    respListBox.Items.Add("Fecha Cupom ...");
                    Application.DoEvents();

                    WriteResp("Finalizado!");
                }
            }
            catch (NullReferenceException)
            {
                messageToolStripStatusLabel.Text = "Não inicializado.";
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (Exception exception)
            {
                messageToolStripStatusLabel.Text = "Exception";
                descriptionToolStripStatusLabel.Text = exception.Message;
                WriteResp(exception.Message);
            }
        }

        private void LeituraX()
        {
            try
            {
                acbrECF.LeituraX();
                WriteResp("LeituaX OK");
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (NullReferenceException)
            {
                messageToolStripStatusLabel.Text = "Não inicializado.";
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (Exception exception)
            {
                messageToolStripStatusLabel.Text = "Exception";
                descriptionToolStripStatusLabel.Text = exception.Message;
            }
        }

        private void ReducaoZ()
        {
            try
            {
                acbrECF.LeituraX();
                WriteResp("ReducaoZ OK");
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (NullReferenceException)
            {
                messageToolStripStatusLabel.Text = "Não inicializado.";
                descriptionToolStripStatusLabel.Text = string.Empty;
            }
            catch (Exception exception)
            {
                messageToolStripStatusLabel.Text = "Exception";
                descriptionToolStripStatusLabel.Text = exception.Message;
            }
        }

        #endregion ECF

        #endregion Methods

        #region Event Handlers

        private void ativarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ativar();
        }

        private void desativarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Desativar();
        }

        private void ativarCheckButton_CheckedChanged(object sender, EventArgs e)
        {
            if (ativarCheckButton.Checked)
            {
                Ativar();
            }
            else
            {
                Desativar();
            }
        }

        private void testarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Testar();
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sair();
        }

        private void estadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ler_Estado();
        }

        private void dataHoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ler_DataHora();
        }

        private void numECFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ler_NumECF();
        }

        private void numLojaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ler_NumLoja();
        }

        private void numSérieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ler_NumSerie();
        }

        private void numVersãoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ler_NumVersao();
        }

        private void cNPJToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ler_CNPJ();
        }

        private void iEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ler_IE();
        }

        private void pAFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ler_PAF();
        }

        private void dadosReducaoZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ler_DadosReducaoZ();
        }

        private void dadosUltimaReduçãoZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ler_DadosUltimaReducaoZ();
        }

        private void lerTodasAsVariáveisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ler_TodasVariaveis();
        }

        private void testaCupomFiscalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestaCupomFiscal();
        }

        private void leituraXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LeituraX();
        }

        private void reToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReducaoZ();
        }

        private void leituraMemóriaFiscalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (LeituraMemoriaFiscal form = new LeituraMemoriaFiscal())
            {
                var ret = form.ShowDialog();
                if (ret == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        if (form.ByPeriod)
                        {
                            this.acbrECF.LeituraMemoriaFiscal(form.InitialDate, form.FinalDate, form.Simple);
                        }
                        else
                        {
                            this.acbrECF.LeituraMemoriaFiscal(form.InitialCCR, form.FinalCCR, form.Simple);
                        }

                        WriteResp("LeituraMemoriaFiscal OK");
                    }
                    catch (NullReferenceException)
                    {
                        messageToolStripStatusLabel.Text = "Não inicializado.";
                        descriptionToolStripStatusLabel.Text = string.Empty;
                    }
                    catch (Exception exception)
                    {
                        messageToolStripStatusLabel.Text = "Exception";
                        descriptionToolStripStatusLabel.Text = exception.Message;
                    }
                }
            }
        }

        private void leituraMemóriaFiscalSerialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (LeituraMemoriaFiscal form = new LeituraMemoriaFiscal())
            {
                var ret = form.ShowDialog();
                if (ret == System.Windows.Forms.DialogResult.OK)
                {
                    string result;

                    try
                    {
                        if (form.ByPeriod)
                        {
                            result = this.acbrECF.LeituraMemoriaFiscalSerial(form.InitialDate, form.FinalDate, form.Simple);
                        }
                        else
                        {
                            result = this.acbrECF.LeituraMemoriaFiscalSerial(form.InitialCCR, form.FinalCCR, form.Simple);
                        }

                        WriteResp(result);
                    }
                    catch (NullReferenceException)
                    {
                        messageToolStripStatusLabel.Text = "Não inicializado.";
                        descriptionToolStripStatusLabel.Text = string.Empty;
                    }
                    catch (Exception exception)
                    {
                        messageToolStripStatusLabel.Text = "Exception";
                        descriptionToolStripStatusLabel.Text = exception.Message;
                    }
                }
            }
        }

        private void identificaPAFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (IdentificaPAF form = new IdentificaPAF())
            {
                var ret = form.ShowDialog();
                if (ret == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        acbrECF.IdentificaPAF(form.Linha1, form.Linha2);
                        WriteResp("Identifica PAF: " + form.Linha1);
                    }
                    catch (NullReferenceException)
                    {
                        messageToolStripStatusLabel.Text = "Não inicializado.";
                        descriptionToolStripStatusLabel.Text = string.Empty;
                    }
                    catch (Exception exception)
                    {
                        messageToolStripStatusLabel.Text = "Exception";
                        descriptionToolStripStatusLabel.Text = exception.Message;
                    }
                }
            }
        }

        private void aacAbrirArquivoButton_Click(object sender, EventArgs e)
        {
            CarregarAAC();
        }

        private void aacGravarArquivoButton_Click(object sender, EventArgs e)
        {
            RecriarAAC();
        }

        private void accVisualizarArquivoButton_Click(object sender, EventArgs e)
        {
            if (File.Exists(aacNomeArquivoTextbox.Text))
            {
                System.Diagnostics.Process.Start(aacNomeArquivoTextbox.Text);
            }
        }

        private void aacRemoverECFAutorizadaButton_Click(object sender, EventArgs e)
        {
            if (aacECFsAutorizadaDataGridView.SelectedRows.Count > 0)
            {
                aacECFsAutorizadaDataGridView.Rows.Remove(aacECFsAutorizadaDataGridView.SelectedRows[0]);
            }
        }

        private void aacAdicionarECFAutorizadaButton_Click(object sender, EventArgs e)
        {
            if (aacNumSerieECFAutorizadaTextBox.Text == "" || aacCROECFAutorizadaTextBox.Text == "" || aacGrandTotalECFAutorizadaTextBox.Text == "" || aacCNIECFAutorizadaTextBox.Text == "")
            {
                MessageBox.Show("Preencha corretamente os valores do ECF a ser autorizado");
            }
            else
            {
                aacECFsAutorizadaDataGridView.Rows.Add(aacNumSerieECFAutorizadaTextBox.Text, aacCROECFAutorizadaTextBox.Text, aacCNIECFAutorizadaTextBox.Text, aacGrandTotalECFAutorizadaTextBox.Text);
                aacNumSerieECFAutorizadaTextBox.Text = "";
                aacCROECFAutorizadaTextBox.Text = "";
                aacGrandTotalECFAutorizadaTextBox.Text = "";
                aacCNIECFAutorizadaTextBox.Text = "";
            }
        }

        private void aacVerificarGTAutorizadaButton_Click(object sender, EventArgs e)
        {
            if (aacECFsAutorizadaDataGridView.SelectedRows.Count > 0)
            {
                if (aacNomeArquivoTextbox.Text != "")
                {
                    acbrAAC.NomeArquivoAuxiliar = aacNomeArquivoTextbox.Text;
                    acbrAAC.Chave = aacChaveTextBox.Text;

                    string numSerie = aacECFsAutorizadaDataGridView.SelectedRows[0].Cells["numSerieColumn"].Value.ToString();
                    double grandTotal = Convert.ToDouble(aacECFsAutorizadaDataGridView.SelectedRows[0].Cells["valorGTColumn"].Value);

                    int retorno = acbrAAC.VerificarGrandeTotal(numSerie, grandTotal);

                    if (retorno == 0)
                    {
                        MessageBox.Show("Os valores estão conferindo. Operação realizada com êxito");
                    }
                    else
                    {
                        MessageBox.Show("Os valores nao conferem");
                    }
                }
                else
                {
                    MessageBox.Show("Coloque o nome do arquivo");
                }
            }
        }

		private void usarAACCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (usarAACCheckBox.Checked)
			{
				this.acbrECF.AAC = acbrAAC;
			}
			else
			{
				this.acbrECF.AAC = null;
			}
		}

        #endregion Event Handlers
    }
}
