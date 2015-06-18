using System;
using System.IO.Compression;
using System.Windows.Forms;

namespace ProblemGenerator
{
    public partial class Form1 : Form
    {
        #region GlobalData

        private string folderName = @"C:\ProblemMake";
        private string pbName;
        private int totalPoints;
        private int testNumber;
        private string pPerTest;
        private string pathString;
        private string zipPath;
        private string testPath;
        private bool zipActive = true;
        private System.IO.FileStream fs;

        #endregion GlobalData

        public Form1()
        {
            InitializeComponent();
            if (!zipActive)
                button2.Enabled = false;
        }

        /// <summary>
        /// Init the data.
        /// </summary>
        private void dataInit()
        {
            pbName = textBox1.Text;
            bool result;

            //Data validation
            result = int.TryParse(textBox2.Text, out totalPoints);
            if (result == false)
            {
                MessageBox.Show("Maximum points not valid int!");
                return;
            }

            result = int.TryParse(textBox3.Text, out testNumber);
            if (result == false)
            {
                MessageBox.Show("Test number not valid int!");
                return;
            }

            if (zipActive)
            {
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                button1.Enabled = false;
            }
            else
                button2.Enabled = false;
            pPerTest = (totalPoints / testNumber).ToString();

            pathString = System.IO.Path.Combine(folderName, pbName);
            zipPath = System.IO.Path.Combine(pathString, "data.zip");
            testPath = pathString + "/tests/";
        }

        /// <summary>
        /// Generates the files, and init.json
        /// </summary>
        private void generate()
        {
            //Init data
            dataInit();

            string init = "{\"archive\":\"data.zip\",\"test_cases\":[";

            for (int i = 0; i < testNumber; i++)
            {
                string inFile = i.ToString() + "-" + pbName + ".in";
                string okFile = i.ToString() + "-" + pbName + ".ok";

                System.IO.Directory.CreateDirectory(testPath);

                //File Make
                string inPathString = System.IO.Path.Combine(testPath, inFile);
                string okPathString = System.IO.Path.Combine(testPath, okFile);

                fs = System.IO.File.Create(inPathString);
                fs.Close();
                fs = System.IO.File.Create(okPathString);
                fs.Close();

                //Init.json test add
                init += "{\"in\": \"" + inFile + "\",";
                init += "\"out\": \"" + okFile + "\",";
                if (i != testNumber - 1)
                    init += "\"points\": \"" + pPerTest + "\"},";
                else
                    init += "\"points\": \"" + pPerTest + "\"}";
            }

            //Finish Init
            init += "]}";
            System.IO.File.WriteAllText(System.IO.Path.Combine(pathString, "init.json"), init);

            MessageBox.Show("Done!\nGenerated under " + pathString);
        }

        /// <summary>
        /// Zip the tests
        /// </summary>
        public void zip()
        {
            try
            {
                ZipFile.CreateFromDirectory(testPath, zipPath);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return;
            }
            button1.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            MessageBox.Show("Done!\nGenerated under " + zipPath);
        }

        /// <summary>
        /// The generate button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            generate();
        }

        /// <summary>
        /// The zip button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            zip();
        }
    }
}