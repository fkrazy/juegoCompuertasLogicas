using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace juegoCompuertasLogicas
{
    class CompuertaNodo
    {
        public static Dictionary<string, int> Tipos = new Dictionary<string, int>
        {
            { "And" , 1 },
            {"Or", 2 },
            {"Not", 3 },
            {"Nand", 4 },
            {"Nor", 5 },
            {"Xor", 6 }
        };

        public int Tipo { get; set; }
        public CompuertaNodo izquierda { get; set; }
        public CompuertaNodo Derecha { get; set; }
        public CompuertaNodo Siguiente { get; set; }
        public Point posicion { get; set; }
        public bool entradaIzquierda { get; set; }
        public bool entradaDerecha { get; set; }
        public int espacio { get; set; }
        public bool Salida { get; private set;  }
        
        public CompuertaNodo(int tipo)
        {
            Tipo = tipo;
            cambioValor(false, false);
            entradaDerecha = false;
            entradaDerecha = false;
        }
        public void cambioValor(bool izquierda, bool derecha )
        {
            entradaDerecha = derecha;
            entradaIzquierda = izquierda;       
            switch (Tipo)
            {
                case 1:
                    if (izquierda && derecha)
                    {
                        Salida = true;
                    }
                    else Salida = false;
                    break;
                case 2:
                    if (izquierda || derecha) Salida = true;
                    else Salida = false;
                    break;
                case 3:
                    if (!izquierda) Salida = true;
                    else Salida = false;
                    break;
                case 4:
                    if (!(izquierda && derecha)) Salida = true;
                    else Salida = false;
                    break;
                case 5:
                    if (!(izquierda || derecha)) Salida = true;
                    else Salida = false;
                    break;
                case 6:
                    if (izquierda ^ derecha) Salida = true;
                    else Salida = false;
                    break;
                default:
                    throw new Exception("tipo no existe");
            }
            if(Siguiente != null)
            {
                Siguiente.actualizar();
            }
        }

        public void actualizar()
        {
            if (Derecha!=null)
            {
                cambioValor(izquierda.Salida, Derecha.Salida);
            }
            else
                cambioValor(izquierda.Salida,false);
        }
    }
}
