using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace jogopalavrascruzadas
{
    class PalavraCruzada
    {
        public String palavra;
        public String dica;
        public List<TextBox> edits;

        public PalavraCruzada()
        {
            edits = new List<TextBox>();
        }

        public void desenhaDica()
        {
            Label lb    = new Label();
            TextBox edt    = edits[edits.Count - 1];
            if (edt == null)
            {
                edt = edits[edits.Count - 2];
            }
            lb.Text     = dica;
           
            lb.Location = new Point(edt.Left + 52, edt.Top + 7);
            lb.Width    = 250;
            edt.Parent.Controls.Add( lb );
        }

        public int solucao()
        {
            int totais = 0;
            for (int x = 0; x < edits.Count; x++)
            {
                if (edits[x] != null) {
                    if (edits[x].Text == palavra[x].ToString())
                    {
                        totais += 1;
                    }
                    edits[x].Text = palavra[x].ToString();
                }                
            }
            return totais;
        }

    }
}
