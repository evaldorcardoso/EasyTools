using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Devices.Sensors;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System.Reflection;
using System.Windows.Media.Imaging;
using System.Threading;




namespace EasyTasks
{
    public partial class Main : PhoneApplicationPage
    {
     
        public Main()
        {
            InitializeComponent();
        }
        Accelerometer acc;
        Boolean soma = false;
        Boolean subtracao = false;
        Boolean divisao = false;
        Boolean multiplicacao = false;        
        Boolean ponto = false;
        bool go = false;


        double altura;
        double peso;
        double telaCalc=0;

        //função para validar campos do peso ideal
        private Boolean validaCampos()
        {
            if ((txtAltura.Text == "") || (txtPeso.Text == ""))
            {
                MessageBox.Show("Algum campo vazio!");
                return false;
            }
            else
            {
                return true;
            }

        }

        //função para validar campos do combustível
        private Boolean validaCamposComb()
        {
            if ((txtGasolina.Text == "") || (txtAlcool.Text == ""))
            {
                MessageBox.Show("Algum campo vazio!");
                return false;
            }
            else
            {
                return true;
            }

        }

        //função para avaliar peso
        private string avaliaPeso(double imc)
        {
            string avaliacao=string.Empty;
            //feminino
            if (rdbFeminino.IsChecked == true)
            {
                if (imc < 19.1)
                    avaliacao = "Abaixo do Peso!";
                else
                {
                    if ((imc >= 19.1) && (imc < 25.8))
                        avaliacao = "Com o peso Normal!";
                    else
                    {
                        if ((imc >= 25.8) && (imc < 27.3))
                            avaliacao = "Acima do peso";
                        else
                        {
                            if ((imc >= 27.3) && (imc < 32.3))
                                avaliacao = "com Obesidade Grau 1";
                            else
                            {
                                if (imc >= 32.3)
                                    avaliacao = "com Obesidade Grau 2";
                            }
                        }
                    }
                }
            }
            //masculino
            else
            {
                if (imc < 20.7)
                    avaliacao = "Abaixo do Peso!";
                else
                {
                    if ((imc >= 20.7) && (imc < 26.4))
                        avaliacao = "Com o peso Normal!";
                    else
                    {
                        if ((imc >= 26.4) && (imc < 27.8))
                            avaliacao = "Acima do peso";
                        else
                        {
                            if ((imc >= 27.8) && (imc < 31.1))
                                avaliacao = "com Obesidade Grau 1";
                            else
                            {
                                if (imc >= 31.1)
                                    avaliacao = "com Obesidade Grau 2";
                            }
                        }
                    }
                }
            }
            return avaliacao;
        }

        //função para avaliar combustível
        private string avaliaCombustivel(double comb)
        {
            string avaliacao = string.Empty;
            if (comb < 70)
                avaliacao = "É mais econômico e preferível abastecer com Álcool!";
            else
            {
                avaliacao = "É mais econômico e preferível abastecer com gasolina!";
            }
            return avaliacao;
        }

        //clique do peso ideal
        private void btnCalcular_Click(object sender, RoutedEventArgs e)
        {
            if (validaCampos())
            {
                try
                {
                    altura = Convert.ToDouble(txtAltura.Text);
                    peso = Convert.ToDouble(txtPeso.Text);
                    
                }
                catch (Exception)
                {
                    MessageBox.Show("Valores inválidos informados!");
                    
                }
                double result;
                result = peso / (altura * altura);
                result =Math.Round(result,1);
                txtbPesoIdeal.Text = "";
                txtbPesoIdeal.Text = "Você está: " + avaliaPeso(result)+" \n Índice de Massa Corporal= "+result.ToString();
                //txtbPesoIdeal.Text = "Seu peso ideal é: " + result.ToString();
            }
            else
            {
                MessageBox.Show("Valores Inválidos!");
            }
        }

        //clique de calcular combustível
        private void btnCalcula_Click(object sender, RoutedEventArgs e)
        {
            if (validaCamposComb())
            {
                double result;
                try
                {
                    altura = Convert.ToDouble(txtGasolina.Text);
                    peso = Convert.ToDouble(txtAlcool.Text);
                    result = (peso / altura)*100;
                    result = Math.Round(result, 0);
                    txtbSolucaoComb.Text = "";
                    txtbSolucaoComb.Text = avaliaCombustivel(result);
                }
                catch (Exception)
                {
                    MessageBox.Show("Valores inválidos informados!");
                }
            }
        }

        //função para setar a imagem dos dados de acordo com o valor selecionado no dado
        private void setaImagem(int x)
        {
            Uri uri = new Uri("Images/" + x.ToString() + ".png", UriKind.Relative);
            ImageSource imgSource = new BitmapImage(uri);
            imgNumSorteado.Source = imgSource;
        }

        //função para jogar os dados
        private void jogando()
        {
            go = true;
            int i = 1;
            int cont = 0;
            while ((go)&&(cont<12))
            {
                Thread.Sleep(100);
                i++;
                cont++;
                if (i == 6)
                {
                    i = 1;
                }
                Deployment.Current.Dispatcher.BeginInvoke(() => atualizaTela(i,false ));
            }
            go = false;
            Deployment.Current.Dispatcher.BeginInvoke(() => atualizaTela(i,true));
        }

        //função para atualizar a tela
        private void atualizaTela(int y,bool a)
        {

            if (!a)
            {
                
                setaImagem(y);
            }
            else
            {
                int x;
                x = 0;
                Random r = new Random();
                x = r.Next(1, 7);
                setaImagem(x);
                txtbToqueDados.Text = "Toque nos dados ou toque no botão acima e mexa o celular para jogar";
            }
        }

        //clique do jogar dados
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            
            if (go == false)
            {
                Thread t = new Thread(jogando);    
                t.Start();
                txtbToqueDados.Text = "Jogando...";
            }
            else
            {
                txtbToqueDados.Text = "Toque nos dados ou toque no botão acima e mexa o celular para jogar";
                go = false;
            }
        }

        //funcao para setar numero na tela da calculadora
        private void numero(string x)
        {
            if (txtTela.Text == "0")
            {
                txtTela.Text = x;
            }
            else
            {
                txtTela.Text = txtTela.Text + x;
            }
        }


        private void btnUm_Click(object sender, RoutedEventArgs e)
        {
            numero("1");
        }

        private void btnDois_Click(object sender, RoutedEventArgs e)
        {
            numero("2");
        }

        private void btnTres_Click(object sender, RoutedEventArgs e)
        {
            numero("3");
        }

        private void btnQuatro_Click(object sender, RoutedEventArgs e)
        {
            numero("4");
        }

        private void btnCinco_Click(object sender, RoutedEventArgs e)
        {
            numero("5");
        }

        private void btnSeis_Click(object sender, RoutedEventArgs e)
        {
            numero("6");
        }

        private void btnSete_Click(object sender, RoutedEventArgs e)
        {
            numero("7");
        }

        private void btnOito_Click(object sender, RoutedEventArgs e)
        {
            numero("8");
        }

        private void btnNove_Click(object sender, RoutedEventArgs e)
        {
            numero("9");
        }

        private void btnZero_Click(object sender, RoutedEventArgs e)
        {
            numero("0");
        }


        private void btnRaiz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                telaCalc = Convert.ToDouble(txtTela.Text);
                txtTela.Text = "0";
                txtbAcao.Text = "√";
                soma = false;
                subtracao = false;
                divisao = false;
                multiplicacao = false;
                telaCalc = Math.Sqrt(telaCalc);
                txtTela.Text = telaCalc.ToString();
            }
            catch (Exception)
            {
                txtTela.Text = "Error";
            }
            
        }

        private void btnDivisao_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                telaCalc = Convert.ToDouble(txtTela.Text);
                txtTela.Text = "0";
                txtbAcao.Text = "/";
                soma = false;
                subtracao = false;
                divisao = true;
                multiplicacao = false;
            }
            catch (Exception)
            {
                
            }
            
        }

        private void btnMultiplicar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                telaCalc = Convert.ToDouble(txtTela.Text);
                txtTela.Text = "0";
                txtbAcao.Text = "x";
                soma = false;
                subtracao = false;
                divisao = false;
                multiplicacao = true;
            }
            catch (Exception)
            {
                
            }
            
        }

        private void btnMenos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                telaCalc = Convert.ToDouble(txtTela.Text);
                txtTela.Text = "0";
                txtbAcao.Text = "-";
                soma = false;
                subtracao = true;
                divisao = false;
                multiplicacao = false;
            }
            catch (Exception)
            {
                
            }
            
        }

        private void btnMais_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                telaCalc = Convert.ToDouble(txtTela.Text);
                txtTela.Text = "0";
                txtbAcao.Text = "+";
                soma = true;
                subtracao = false;
                divisao = false;
                multiplicacao = false;
            }
            catch (Exception)
            {

            }
            
        }

        //clique do botao igual
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (soma)
            {
                telaCalc = telaCalc + Convert.ToDouble(txtTela.Text);
            }
            if (subtracao)
            {
                telaCalc = telaCalc - Convert.ToDouble(txtTela.Text);
            }
            if (divisao)
            {
                telaCalc = telaCalc / Convert.ToDouble(txtTela.Text);
            }
            if (multiplicacao)
            {
                telaCalc = telaCalc * Convert.ToDouble(txtTela.Text);
            }

            txtbAcao.Text ="";
            txtTela.Text = telaCalc.ToString();
            soma = false;
            subtracao = false;
            divisao = false;
            multiplicacao = false;
        }

        private void btnLimpar_Click(object sender, RoutedEventArgs e)
        {
            txtTela.Text = "0";
            txtbAcao.Text = "";
            telaCalc = 0;
            soma = false;
            subtracao = false;
            divisao = false;
            multiplicacao = false;
            ponto = false;
        }

        private void btnVirgula_Click(object sender, RoutedEventArgs e)
        {
            if (ponto)
            { 

            }
            else
            {
                ponto = true;
                numero(".");
            }
            

        }

        //clique para ligar o acelerômetro
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (acc == null)
            {
                acc = new Accelerometer();
                acc.ReadingChanged += new EventHandler<AccelerometerReadingEventArgs>(acc_ReadingChanged);
                try
                {
                    txtbStatusAcelerômetro.Text = "Acelerômetro Iniciado";
                    acc.Start();
                    txtbToqueDados.Text = "Toque nos dados ou mexa o celular para jogar";
                }
                catch (AccelerometerFailedException)
                {
                    txtbStatusAcelerômetro.Text = "Erro ao iniciar o Acelerômetro";
                }
            }
            else
            {
                try
                {
                    acc.Stop();
                    acc = null;
                    txtbStatusAcelerômetro.Text = "Acelerômetro Parado";
                    txtbToqueDados.Text = "Toque nos dados ou toque no botão acima e mexa o celular para jogar";
                }
                catch (AccelerometerFailedException)
                {
                    txtbStatusAcelerômetro.Text = "Erro ao parar o Acelerômetro";
                }
            }
        }

        void acc_ReadingChanged(object sender, AccelerometerReadingEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => minhaLeitura(e));
        }

        void minhaLeitura(AccelerometerReadingEventArgs e)
        {
            if (acc != null)
            {
                //txtbToqueDados.Text = e.Y.ToString() + "   " +e.Z.ToString();
                if (e.Z < -0.4)
                {
                    if (go == false)
                    {
                        Thread t = new Thread(jogando);
                        t.Start();
                        txtbToqueDados.Text = "Jogando...";
                    }
                    else
                    {
                        txtbToqueDados.Text = "Toque nos dados ou toque no botão acima e mexa o celular para jogar";
                        go = false;
                    }
                    acc.Stop();
                    acc = null;
                    txtbStatusAcelerômetro.Text = "Acelerômetro parado";
                }
            }

        }

        

  
        
    }
}