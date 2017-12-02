
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace JuegoMemorama
{
    
    public partial class Form1 : Form
    {
        Boolean tiempo_logrado = false;
        Boolean abierto = false;
        int tiempo_sinSuperar = 60;
        int tiempo_dificil = 60;
        int tiempo_dificil2 = 30;
        int TamanioColumnasFilas = 4;
        int Movimientos = 0;
        int CantidadDeCartasVolteadas = 0;
        List<string> CartasEnumeradas;
        List<string> CartasRevueltas;
        ArrayList CartasSeleccionadas;
        PictureBox CartaTemporal1;
        PictureBox CartaTemporal2;
        int CartaActual = 0;



        public Form1()
        {
            
            InitializeComponent();
            iniciarJuego();
            Mostrar();
                 
        }
        public void iniciarJuego() {

            tiempo_logrado = false;
            timer1.Enabled = false;
            timer1.Stop();
            lblRecord.Text = "0";
            CantidadDeCartasVolteadas = 0;
            Movimientos = 0;
            PanelJuego.Controls.Clear();
            CartasEnumeradas= new List<string>();
            CartasRevueltas = new List<string>();
            CartasSeleccionadas = new ArrayList();
            for (int i = 0; i < 8; i++) {
                CartasEnumeradas.Add(i.ToString());
                CartasEnumeradas.Add(i.ToString());
            }
            var NumeroAleatorio = new Random();
            var Resultado = CartasEnumeradas.OrderBy(item=> NumeroAleatorio.Next());
            foreach(string ValorCarta in Resultado){
                CartasRevueltas.Add(ValorCarta);
            }
            var tablaPanel = new TableLayoutPanel();
            tablaPanel.RowCount = TamanioColumnasFilas;
            tablaPanel.ColumnCount = TamanioColumnasFilas;
            for (int i = 0; i < TamanioColumnasFilas;i++ )
            {
                var Porcentaje = 150f / (float)TamanioColumnasFilas - 10;
                tablaPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,Porcentaje));
                tablaPanel.RowStyles.Add(new RowStyle(SizeType.Percent,Porcentaje));
             }
            int contadorFichas = 1;

            for (var i = 0; i < TamanioColumnasFilas; i++)
            {
                for (var j = 0; j < TamanioColumnasFilas; j++)
                {
                    var CartasJuego = new PictureBox();
                    CartasJuego.Name = string.Format("{0}", contadorFichas);
                    CartasJuego.Dock = DockStyle.Fill;
                    CartasJuego.SizeMode = PictureBoxSizeMode.StretchImage;
                    CartasJuego.Image = Properties.Resources.Girada;
                    CartasJuego.Cursor = Cursors.Hand;
                    CartasJuego.Click += btnCarta_Click;
                    tablaPanel.Controls.Add(CartasJuego, j, i);
                    contadorFichas++;
                }
            }
            tablaPanel.Dock = DockStyle.Fill;
            PanelJuego.Controls.Add(tablaPanel);

        
        }
        private void btnReiniciar_Click(object sender, EventArgs e)
        {
            iniciarJuego();
        }
        private void btnCarta_Click(object sender, EventArgs e)
        {
            if (CartasSeleccionadas.Count < 2) { 
            Movimientos++;
            lblRecord.Text = Convert.ToString(Movimientos);
            var CartasSeleccionadasUsuario = (PictureBox)sender;

            CartaActual= Convert.ToInt32(CartasRevueltas[Convert.ToInt32(CartasSeleccionadasUsuario.Name)-1]);
            CartasSeleccionadasUsuario.Image = RecuperarImagen(CartaActual);
            CartasSeleccionadas.Add(CartasSeleccionadasUsuario);
                //  2 Veces se realizo el evento del click
                if(CartasSeleccionadas.Count==2){
                    CartaTemporal1 = (PictureBox)CartasSeleccionadas[0];
                    CartaTemporal2 = (PictureBox)CartasSeleccionadas[1];
                    int Carta1= Convert.ToInt32(CartasRevueltas[Convert.ToInt32(CartaTemporal1.Name)-1]);
                    int Carta2 = Convert.ToInt32(CartasRevueltas[Convert.ToInt32(CartaTemporal2.Name) - 1]);
                    

                    if (Carta1 != Carta2)
                    {
                        timer1.Enabled = true;
                        timer1.Start();
                    }
                    else {
                        CantidadDeCartasVolteadas++;
                        if(CantidadDeCartasVolteadas>7){
                            MessageBox.Show("El juego termino");
                            Agregar(nombre_form1.Text, Movimientos);
                            Tabla_datos.Rows.Add(nombre_form1.Text + "", Movimientos);
                            timer2.Stop();
                        }
                        CartaTemporal1.Enabled = false; CartaTemporal2.Enabled = false;
                        CartasSeleccionadas.Clear();

                    }


                }
            }

        }
        public Bitmap RecuperarImagen(int NumeroImagen){
        Bitmap TmpImg= new Bitmap(200,100);
        switch (NumeroImagen) {
            case 0: TmpImg = Properties.Resources.img11;
                break;
            default: TmpImg = (Bitmap)Properties.Resources.ResourceManager.GetObject("img"+NumeroImagen);
                break;
        }
        return TmpImg;
        
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int TiempoVirarCarta = 1;
            if (TiempoVirarCarta == 1) { 
            CartaTemporal1.Image=Properties.Resources.Girada;
            CartaTemporal2.Image = Properties.Resources.Girada;
            CartasSeleccionadas.Clear();
            TiempoVirarCarta = 0;
            timer1.Stop();
            
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer2.Start();
            iniciarJuego();


        }

        private void timer2_Tick(object sender, EventArgs e)
        {
          

            if (tiempo_logrado == true)
            {
                
            }
            else
            {
                tiempo_dificil = tiempo_dificil - 1;
                if (tiempo_dificil == 0)
                {
                    label3.Text = System.Convert.ToString(tiempo_dificil2);
                    tiempo_dificil2 = tiempo_dificil2 - 1;
                    tiempo_dificil = 60;
                    if (tiempo_dificil2 == 0)
                    {
                        
                        tiempo_logrado = true;
                        label3.Text = "0";
                        tiempo_dificil2 = tiempo_sinSuperar;
                        tiempo_sinSuperar = tiempo_sinSuperar + 30;
                        iniciarJuego();
                        MessageBox.Show("Ohh.. No lo has logrado :(, Te reducire la dificultad");
                    }
                }
            }
            
        }
        public static MySqlConnection ObtenerConexion()
        {
            MySqlConnection conectar = new MySqlConnection("server=localhost; database=desarrollointerfaces; Uid=root; pwd=;");
            conectar.Open();
            return conectar;
        }
        public static int Agregar(String nombre, int record)
        {
            int retorno = 0;

            MySqlCommand comando = new MySqlCommand(string.Format("Insert into puntuacion (Nombre, Record) values ('{0}','{1}')",
                nombre, record), ObtenerConexion());
            retorno = comando.ExecuteNonQuery();
            return retorno;
        }
        public void Mostrar()
        {
            int Id, recor, max=0;
            String nom;

            MySqlCommand _comando = new MySqlCommand(String.Format(
           "SELECT * FROM puntuacion"), ObtenerConexion());
            MySqlDataReader _reader = _comando.ExecuteReader();
            while (_reader.Read())
            {
   
                Id = _reader.GetInt32(0);
                nom = _reader.GetString(1);
                recor = _reader.GetInt32(2);

                Tabla_datos.Rows.Add(nom + "", recor);
            }

            
        }

        public void nombre_form1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
