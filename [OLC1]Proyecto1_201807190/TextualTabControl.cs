using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _OLC1_Proyecto1_201807190
{
    class TextualTabControl : TabControl
    {
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            e.Control.Controls.Add(CreateBox());
        }

        private RichTextBox CreateBox()
        {
            RichTextBox newText = new RichTextBox();
            newText.Dock = DockStyle.Fill;
            newText.Font = new Font("Microsoft Sans Serif", 11.2f);
            return newText;
        }

        public String SelectedRichTextBoxTex
        {
            get { return this.SelectedTab.Controls[0].Text; }
        }
    }
}
