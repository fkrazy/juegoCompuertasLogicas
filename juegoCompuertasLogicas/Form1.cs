using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace juegoCompuertasLogicas
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        private int compuertasFaltantes,profundidad;
        private Random random = new Random();
        private CompuertaNodo raiz;
        List<Point> posicionBotones;
        struct parBoton
        {

            public CompuertaNodo compuerta;
            public bool izquierda;
        }
        Dictionary<Point, parBoton> parBotonDiccionario;
        DateTime inicio;
        private void GenerarButton_Click(object sender, EventArgs e)
        {
            raiz = new CompuertaNodo(random.Next(1, 6));
            parBotonDiccionario = new Dictionary<Point, parBoton>();
            compuertasFaltantes = (int)numericUpDownNivel.Value;
            int distanciaAlto, distanciaBajo;
            distanciaAlto = 54;
            distanciaBajo = 22;
            bool entrar = true;
            while (raiz.Salida || entrar)
            {
                entrar = false;
                raiz = new CompuertaNodo(random.Next(1, 6));
                compuertasFaltantes = (int)numericUpDownNivel.Value;
                while (compuertasFaltantes > 0)
                {                              
                    profundidad = 0;
                    aumentarProfundidar = true;
                    RecorrerArbolYCrear(raiz);
                }
            }                        
            pictureBox1.Image = new Bitmap((profundidad + 3) * 150,(int)Math.Pow(2,profundidad+3)*50);
            label1.Text = profundidad.ToString();
            posicionBotones = new List<Point>();
            raiz.posicion = new Point(pictureBox1.Width - distanciaAlto, pictureBox1.Height / 2 - distanciaBajo);
            raiz.espacio = pictureBox1.Height / 2;
            RecorrerYDibujar(raiz);
            inicio = DateTime.Now;
        }

        private void RecorrerYDibujar(CompuertaNodo compuerta)
        {
            int distanciaX, distanciaY;
            distanciaX = 132;
            if (compuerta.Siguiente != null)
            {
                distanciaY = compuerta.posicion.Y;
            }
            else
            {
                distanciaY = pictureBox1.Height / 2 - 22;
            }
            DibujarCompuerta(compuerta);

            if (compuerta.izquierda != null)
            {
                compuerta.izquierda.posicion = new Point(compuerta.posicion.X - distanciaX, distanciaY - compuerta.espacio / 2);
                compuerta.izquierda.espacio = compuerta.espacio / 2;
                RecorrerYDibujar(compuerta.izquierda);
            }
            else
            {
                DibujarBoton(compuerta, true);
            }
            if (compuerta.Derecha != null)
            {
                compuerta.Derecha.posicion = new Point(compuerta.posicion.X - distanciaX, distanciaY + compuerta.espacio / 2);
                compuerta.Derecha.espacio = compuerta.espacio / 2;
                RecorrerYDibujar(compuerta.Derecha);
            }
            else
            {
                if (compuerta.Tipo != CompuertaNodo.Tipos["Not"])
                {
                    DibujarBoton(compuerta, false);
                }
            }
        }

        private void DibujarBoton(CompuertaNodo compuerta, bool izquierdaViene)
        {
            Graphics g;
            Bitmap imagen = (Bitmap)pictureBox1.Image.Clone();
            g = Graphics.FromImage(imagen);
            int distanciaY = compuerta.posicion.Y;
            int distanciaFinalY;
            if (izquierdaViene)
            {
                distanciaFinalY = distanciaY - compuerta.espacio / 2;
            }
            else
            {
                distanciaFinalY = distanciaY + compuerta.espacio / 2;
            }
            g.DrawRectangle(Pens.Black, compuerta.posicion.X - 132, distanciaFinalY, 33, 33);
            g.DrawRectangle(Pens.Black, compuerta.posicion.X - 127, distanciaFinalY + 5, 22, 22);

            Point posBoton = new Point(compuerta.posicion.X - 132, distanciaFinalY);
            posicionBotones.Add(posBoton);
            parBoton parBoton = new parBoton
            {
                compuerta = compuerta,
                izquierda = izquierdaViene
            };
            parBotonDiccionario.Add(posBoton, parBoton);
            try
            {
                g.DrawLine(new Pen(Brushes.Black), compuerta.posicion.X - 90, distanciaFinalY + 18, compuerta.posicion.X, compuerta.posicion.Y + 18);
            }
            catch (Exception)
            {

            }
            pictureBox1.Image = imagen;
            g.Dispose();
        }

        private void DibujarCompuerta(CompuertaNodo compuerta)
        {
            Graphics g;
            Bitmap imagen = (Bitmap)pictureBox1.Image.Clone();

            g = Graphics.FromImage(imagen);
            Brush pincel;
            if (compuerta.Salida)
            {
                pincel = Brushes.Red;                
            }
            else
            {
                pincel = Brushes.Black;
            }
            g.FillRectangle(pincel, compuerta.posicion.X, compuerta.posicion.Y, 44, 44);
            switch (compuerta.Tipo)
            {
                case 1:
                    g.DrawImage(juegoCompuertasLogicas.Properties.Resources.and, compuerta.posicion);
                    break;
                case 2:
                    g.DrawImage(juegoCompuertasLogicas.Properties.Resources.or, compuerta.posicion);
                    break;
                case 3:
                    g.DrawImage(juegoCompuertasLogicas.Properties.Resources.not, compuerta.posicion);
                    break;
                case 4:
                    g.DrawImage(juegoCompuertasLogicas.Properties.Resources.nand, compuerta.posicion);
                    break;
                case 5:
                    g.DrawImage(juegoCompuertasLogicas.Properties.Resources.nor, compuerta.posicion);
                    break;
                case 6:
                    g.DrawImage(juegoCompuertasLogicas.Properties.Resources.xor, compuerta.posicion);
                    break;
            };
            if (compuerta.Siguiente != null)
            {
                g.DrawLine(new Pen(Brushes.Black), compuerta.posicion.X + 44, compuerta.posicion.Y + 22, compuerta.Siguiente.posicion.X, compuerta.Siguiente.posicion.Y + 18);
            }
            pictureBox1.Image = imagen;
            g.Dispose();
        }
        bool aumentarProfundidar = true;
        private void RecorrerArbolYCrear(CompuertaNodo compuerta)
        {
            if (compuerta.izquierda != null)
            {
                if (aumentarProfundidar)
                {
                    profundidad++;
                };
                RecorrerArbolYCrear(compuerta.izquierda);
                if (compuerta.Derecha != null)
                    RecorrerArbolYCrear(compuerta.Derecha);
                else if (compuerta.Tipo != CompuertaNodo.Tipos["Not"])
                {
                    CrearCompuerta(compuerta);
                }
            }
            else
            {
                aumentarProfundidar = false;
                CrearCompuerta(compuerta);
            }
           
        }

        private void CrearCompuerta(CompuertaNodo compuerta)
        {
            compuertasFaltantes--;
            compuerta.izquierda = new CompuertaNodo(random.Next(1, 6));
            compuerta.izquierda.Siguiente = compuerta;
            if (compuerta.Tipo != CompuertaNodo.Tipos["Not"] && compuertasFaltantes > 0)
            {
                compuertasFaltantes--;
                compuerta.Derecha = new CompuertaNodo(random.Next(1, 6));
                compuerta.Derecha.Siguiente = compuerta;
            }
            compuerta.actualizar();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }
        int mouseX, mouseY;

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            #region actualizar boton
            foreach (Point posicion in posicionBotones)
            {
                int diferenciaX = mouseX - posicion.X;
                int diferenciaY = mouseY - posicion.Y;
                Brush paleta;
                if (diferenciaX <= 44 && diferenciaY <= 44 && diferenciaX >= 0 && diferenciaY >= 0)
                {
                    if (parBotonDiccionario[posicion].izquierda)
                    {
                        parBotonDiccionario[posicion].compuerta.cambioValor(!parBotonDiccionario[posicion].compuerta.entradaIzquierda, parBotonDiccionario[posicion].compuerta.entradaDerecha);
                        if (parBotonDiccionario[posicion].compuerta.entradaIzquierda)
                        {
                            paleta = Brushes.Red;
                        }
                        else
                        {
                            paleta = Brushes.Black;
                        }
                    }
                    else
                    {
                        parBotonDiccionario[posicion].compuerta.cambioValor(parBotonDiccionario[posicion].compuerta.entradaIzquierda, !parBotonDiccionario[posicion].compuerta.entradaDerecha);
                        if (parBotonDiccionario[posicion].compuerta.entradaDerecha)
                        {
                            paleta = Brushes.Red;
                        }
                        else
                        {
                            paleta = Brushes.Black;
                        }
                    };
                    Graphics g;
                    Bitmap imagen = (Bitmap)pictureBox1.Image.Clone();
                    g = Graphics.FromImage(imagen);
                    g.FillRectangle(paleta, posicion.X + 8, posicion.Y + 8, 15, 15);
                    pictureBox1.Image = imagen;
                    g.Dispose();
                    CompuertaNodo aux = parBotonDiccionario[posicion].compuerta;
                    DibujarCompuerta(parBotonDiccionario[posicion].compuerta);
                    while (aux.Siguiente != null)
                    {
                        DibujarCompuerta(aux.Siguiente);
                        aux = aux.Siguiente;
                    }
                    break;
                } 
            }         
            #endregion
            if(raiz.Salida)
            {
                ScoreLabel.Text = $"Score: {(double)numericUpDownNivel.Value *1000/(DateTime.Now - inicio).TotalSeconds}";
                MessageBox.Show("usted a ganado");
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;
        }
    }
}
