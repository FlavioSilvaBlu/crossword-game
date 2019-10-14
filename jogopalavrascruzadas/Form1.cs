using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jogopalavrascruzadas
{
    public partial class FPrincipal : Form
    {

        private int CT_Posicao_central = 190;
        private SqlConnection conn;
        private List<PalavraCruzada> palavras;
        PalavraCruzada palavraPrincipal;
        PalavraCruzada palavraSecundaria;
        PalavraCruzada palavraTerciaria;

        public FPrincipal()
        {
            InitializeComponent();
        }       

        private void sairToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Application.MessageLoop)
            {
                // WinForms app
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                // Console app
                System.Environment.Exit(1);
            }
        }

        private void sobreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("jogo de Palavras Cruzadas\nDesenvolvido por Flávio da Silva 2018", 
                "Sobre", MessageBoxButtons.OK);
        }

        private void cadastroToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void palavrasCruzadasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.Show();
        }

        private void FPrincipal_Load(object sender, EventArgs e)
        {
            String cortar = "bin\\Debug\\jogopalavrascruzadas.exe";
            String path_exe = System.Reflection.Assembly.GetExecutingAssembly().Location;
            String path_db = path_exe.Replace(cortar, "dados\\dados.mdf");
            
            conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + path_db +
                                     ";Integrated Security=True;Connect Timeout=30");
            conn.Open();

        }

        private List<PalavraCruzada> GetPalavras()
        {
            palavras = new List<PalavraCruzada>();
            PalavraCruzada palavra;

            SqlCommand sql = new SqlCommand("Select * from [dbo].[palavras] order by palavra", conn);
            SqlDataReader sqlr = sql.ExecuteReader();

            Boolean aceito = false;
            while (sqlr.Read())
            {
                aceito = true;
                palavra = new PalavraCruzada();
                palavra.dica = (String) sqlr["dica"];
                palavra.palavra = (String) sqlr["palavra"];
                palavras.Add(palavra);               
            }
            sqlr.Close();
            return palavras;
        }

        private void executarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //int txtno = int.Parse(txt1.Text);
                List<PalavraCruzada> palavras = GetPalavras();
                Random rnd = new Random();
                
                int palavra1 = rnd.Next(0, palavras.Count);
                int palavra2 = rnd.Next(0, palavras.Count);
                int palavra3 = rnd.Next(0, palavras.Count);
                int tentativa = 0; 
                palavraPrincipal = palavras[palavra1];
                while (palavraPrincipal.palavra.Length < 4 && tentativa <= 5) 
                {
                    palavra1 = rnd.Next(0, palavras.Count);
                    palavraPrincipal = palavras[palavra1];
                    ++tentativa;
                }
                ImprimePalavraVertical(palavraPrincipal);

                tentativa = 0;

                palavraSecundaria = palavras[palavra2];
                palavraTerciaria  = palavras[palavra3];

                while ((palavra1 == palavra2 || 
                        palavra2 == palavra3 || 
                        palavra3 == palavra1) && tentativa <= 50)
                {
                    palavra2 = rnd.Next(0, palavras.Count);
                    palavraSecundaria = palavras[palavra2];
                    palavra3 = rnd.Next(0, palavras.Count);
                    palavraTerciaria = palavras[palavra3];
                    ++tentativa;
                }

                int idx = ImprimePalavraHorizontal(palavraPrincipal, palavraSecundaria, 2, -1);
                ImprimePalavraHorizontal(palavraPrincipal, palavraTerciaria, 4, idx);
            }
            catch (Exception)
            {
                MessageBox.Show(e.ToString());
            }

            
        }

        private bool isVogal(String teste)
        {
            bool isVowel = "aeiouAEIOUáéíóúãôê".IndexOf(teste) >= 0;
            return isVowel;
        }

        private int ImprimePalavraHorizontal(PalavraCruzada p, PalavraCruzada phorizontal, int letra, int posIgnorar)
        {
            try
            {
                int txtno = phorizontal.palavra.Length;
                int topoletra = letra;
                // achar a primeira vogal ou última dependendo do parâmetro letra

                String vogalInicio = "";
                int vogalInicioPos = 0;
                int z;
                if (letra == 2)
                {
                    z = 0;
                    while (z <= (p.palavra.ToString().Length -1 ))
                    {
                        if (isVogal(p.palavra[z].ToString())) {

                            vogalInicio    = p.palavra[z].ToString();
                            vogalInicioPos = phorizontal.palavra.IndexOf(vogalInicio);
                            topoletra = z;

                            if (vogalInicioPos != -1 && posIgnorar != vogalInicioPos)
                            {
                                z = 999;
                            }                            
                        }   
                        z += 1;
                    }
                } else
                {
                    z = p.palavra.ToString().Length-1;
                    while (z >= 0)
                    {
                        if (isVogal(p.palavra[z].ToString()))
                        {

                            vogalInicio    = p.palavra[z].ToString();
                            vogalInicioPos = phorizontal.palavra.LastIndexOf(vogalInicio);
                            topoletra = z;
                            
                            if (vogalInicioPos != -1 && posIgnorar != vogalInicioPos)
                            {
                                z = -999;
                            }
                        }
                        z -= 1;
                    }
                }                
                

                int pointX = CT_Posicao_central - (vogalInicioPos * 25);
                int pointY = 40;
                

                for (int i = 0; i < txtno; i++)
                {

                    if (vogalInicioPos != i)
                    {
                        pointY = (topoletra) * 25 + 40;
                        TextBox a = new TextBox();
                        //a.Text = phorizontal.palavra[i].ToString();
                        a.Tag  = i;
                        a.MaxLength = 1;
                        a.Location = new Point(pointX, pointY);
                        a.Width = 25;
                        pnlGame.Controls.Add(a);
                        phorizontal.edits.Add(a);
                        

                    } else
                    {
                        phorizontal.edits.Add(null);
                    }

                    pointX += 25;
                }

                return vogalInicioPos;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return 0;
            }
        }

        private void ImprimePalavraVertical(PalavraCruzada p)
        {
            try
            {
                int txtno = p.palavra.Length;
                int pointX = CT_Posicao_central;
                int pointY = 40;
                pnlGame.Controls.Clear();

                for (int i = 0; i < txtno; i++)
                {
                    TextBox a = new TextBox();

                    //a.Text = p.palavra[i].ToString();
                    a.Tag = i;
                    a.MaxLength = 1;
                    a.Location = new Point(pointX, pointY);
                    a.Width = 25;
                    pnlGame.Controls.Add(a);
                    
                    p.edits.Add(a);

                    pointY += 25;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void mnDica_Click(object sender, EventArgs e)
        {
            palavraPrincipal.desenhaDica();
            palavraSecundaria.desenhaDica();
            palavraTerciaria.desenhaDica();
        }

        private void mnSolucao_Click(object sender, EventArgs e)
        {
            int totais = 0;
            totais = palavraPrincipal.solucao();
            totais += palavraSecundaria.solucao();
            totais += palavraTerciaria.solucao();
            lb_pontos.Text = totais.ToString();
        }
    }
}
