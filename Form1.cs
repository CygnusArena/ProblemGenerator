﻿using System;
using System.Windows.Forms;

namespace ProblemGenerator
{
    public partial class Form1 : Form
    {
        #region GlobalData

        private string folderName = @"C:\ProblemMake";

        #endregion GlobalData

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Generates the files, and init.json
        /// </summary>
        private void generate()
        {

            //Getting data 
            string pbName = textBox1.Text;
            int totalPoints;
            int testNumber;
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

            string pPerTest = (totalPoints / testNumber).ToString();

            string pathString = System.IO.Path.Combine(folderName, pbName);
            System.IO.Directory.CreateDirectory(pathString);

            string init = "{\"archive\":\"data.zip\",\"test_cases\":[";

            for (int i = 0; i < testNumber; i++)
            {
                string inFile = i.ToString() + "-" + pbName + ".in";
                string okFile = i.ToString() + "-" + pbName + ".ok";

                //File Make
                string inPathString = System.IO.Path.Combine(pathString, inFile);
                string okPathString = System.IO.Path.Combine(pathString, okFile);

                System.IO.File.Create(inPathString);
                System.IO.File.Create(okPathString);

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

            MessageBox.Show("Done!");
        }

        /// <summary>
        /// The button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            generate();
        }
    }
}