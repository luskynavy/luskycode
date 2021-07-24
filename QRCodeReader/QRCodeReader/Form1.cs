using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace QRCodeReader
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				//demande une image
				using (OpenFileDialog openFileDialog = new OpenFileDialog())
				{
					//jpg et png ou tous les fichiers
					openFileDialog.Filter = "Images jpg, png (*.jpg;*.png)|*.jpg;*.png|Tous les fichiers (*.*)|*.*";

					//si l'utilisateur a choisi un fichier
					if (openFileDialog.ShowDialog() == DialogResult.OK)
					{
						//recupère le chemin
						string filePath = openFileDialog.FileName;

						//affiche l'image
						Bitmap bitamp = new Bitmap(filePath);

						pictureBox1.Image = bitamp;

						//initialise le décodeur
						IBarcodeReader reader = new BarcodeReader();

						//décodage
						var result = reader.Decode(bitamp);
						//si le décodeur a trouvé un texte
						if (result != null)
						{
							textBox1.Text = result.Text;
						}
						//sinon on efface le texte prédécent
						else
						{
							textBox1.Text = "";
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}
	}
}