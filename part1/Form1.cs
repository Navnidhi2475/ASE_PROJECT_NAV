using System;
using System.Drawing;
using System.Windows.Forms;

namespace part1
{
    public partial class Form1 : Form
    {
        private CommandParser parser;

        public Form1()
        {
            InitializeComponent();
            parser = new CommandParser(codeTextBox, displayArea);
            displayArea.Paint += new PaintEventHandler(displayArea_Paint);
            commandTextBox.KeyUp += new KeyEventHandler(commandTextBox_KeyUp);
            runButton.Click += new EventHandler(click_Run);
            syntaxButton.Click += new EventHandler(click_Syntax);
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Initialization that occurs when the form loads can be placed here.
        }

        private void click_Run(object sender, EventArgs e)
        {
            try
            {
                string program = codeTextBox.Text;
                parser.ExecuteProgram(program);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error executing the program: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void click_Syntax(object sender, EventArgs e)
        {
            try
            {
                parser.CheckSyntax();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Syntax error: " + ex.Message, "Syntax Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt";
                saveFileDialog.DefaultExt = "txt";
                saveFileDialog.AddExtension = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    parser.SaveProgram(saveFileDialog.FileName);
                }
            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text Files (*.txt)|*.txt";
                openFileDialog.DefaultExt = "txt";
                openFileDialog.AddExtension = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    parser.LoadProgram(openFileDialog.FileName);
                }
            }
        }


        private void commandTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !e.Handled)
            {
                e.Handled = true; // Mark the event as handled to prevent it from being processed again
                string commandText = commandTextBox.Text.Trim();

                if (string.IsNullOrEmpty(commandText))
                {
                    MessageBox.Show("Please enter a command.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    try
                    {
                        parser.ExecuteCommand(commandText);
                        displayArea.Invalidate(); // Refresh the canvas after executing the command
                        MessageBox.Show("Command executed successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        commandTextBox.Clear(); // Clear the commandTextBox after processing the command
                    }
                }
                commandTextBox.Focus(); // Set focus back to the commandTextBox for new input
            }
        }




        private void displayArea_Paint(object sender, PaintEventArgs e)
        {
            parser.SetupGraphics(e);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            parser.Cleanup();
        }
    }
}
