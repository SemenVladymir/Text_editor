using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Text_editor
{
    public partial class Form1 : Form
    {
        const string fileName = "TextEditor.txt";
        string sample = "";
        ChangeMemory changes = new ChangeMemory(true, "", 0);
        MemoryStack MyStack = new MemoryStack();
        bool bUndo = false;
        bool bRedo = false;
        bool capSymbol = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            settings = new Label();
            settings.Text = "SETTINGS";
            settings.TextAlign = ContentAlignment.TopCenter;
            settings.Location = new Point(5, 0);
            

            Font = new Label();
            Font.Text = "Font size:";
            Font.Size = new Size(80, 50);
            Font.TextAlign = ContentAlignment.MiddleLeft;
            Font.Location = new Point(5, 20);
            Font.BorderStyle = BorderStyle.FixedSingle;
            Font.BackColor = Color.GhostWhite;

            fontSize = new TextBox();
            fontSize.Width = 25;
            fontSize.Location = new Point(50, 15);
            fontSize.TextChanged += FontSize_TextChanged;
            Font.Controls.Add(fontSize);
            settings.Controls.Add(Font);


            alignment = new Panel();
            alignment.Text = "Text alignment";
            alignment.Location = new Point(Font.Width + 30, 20);
            alignment.Size = new Size(170, 50);
            alignment.BorderStyle = BorderStyle.FixedSingle;
            alignment.BackColor = Color.GhostWhite;

            alig = new Label();
            alig.Text = "Text alignment";
            alig.Size = new Size(alig.Text.Length * 12, 12);
            alig.Location = new Point(alignment.Width / 2 - 40, 0);
            alignment.Controls.Add(alig);

            left = new RadioButton();
            left.Text = "Left";
            left.Checked = true;
            left.Click += Left_Click;
            left.Size = new Size(left.Text.Length * 12, 30);
            left.Location = new Point(0, 14);
            alignment.Controls.Add(left);

            center = new RadioButton();
            center.Text = "Center";
            center.Click += Center_Click;
            center.Size = new Size(center.Text.Length * 12, 30);
            center.Location = new Point(left.Width, 14);
            alignment.Controls.Add(center);

            right = new RadioButton();
            right.Text = "Right";
            right.Click += Right_Click;
            right.Size = new Size(right.Text.Length * 12, 30);
            right.Location = new Point(left.Width + center.Width, 14);
            alignment.Controls.Add(right);

            find = new Label();
            find.Size = new Size(200, 50);
            find.TextAlign = ContentAlignment.MiddleLeft;
            find.Location = new Point(305, 20);
            find.BorderStyle = BorderStyle.FixedSingle;
            find.BackColor = Color.GhostWhite;

            search = new TextBox();
            search.Size = new Size(120, 12);
            search.Location = new Point(2, 2);
            find.Controls.Add(search);
            settings.Controls.Add(find);

            change = new TextBox();
            change.Size = new Size(120, 12);
            change.Location = new Point(2, 26);
            find.Controls.Add(change);
            

            bChange = new Button();
            bChange.Text = "Change";
            bChange.Size = new Size(70, 30);
            bChange.Location = new Point(125, 10);
            find.Controls.Add(bChange);
            settings.Controls.Add(find);
            bChange.Click += BChange_Click;

            textBox = new TextBox();
            textBox.Width = Width - 35;
            textBox.Height = Height - 200;
            textBox.Location = new Point(10, 80);
            textBox.Font = new Font(textBox.Font.FontFamily, 12);
            textBox.ScrollBars = ScrollBars.Both;
            textBox.WordWrap = true;
            textBox.Multiline = true;
            textBox.AcceptsTab = true;
            textBox.TextChanged += TextBox_TextChanged;
            Controls.Add(textBox);


            correct = new Label();
            correct.Text = "Cancellation";
            correct.TextAlign = ContentAlignment.TopCenter;
            correct.Size = new Size(200, 50);
            correct.Location = new Point(Font.Width + alignment.Width + find.Width + 70, 20);
            correct.BorderStyle = BorderStyle.FixedSingle;
            correct.BackColor = Color.GhostWhite;

            undo = new Button();
            undo.Text = "Undo";
            undo.Size = new Size(80, 30);
            undo.Location = new Point(5, 14);
            undo.Font = new Font(textBox.Font.FontFamily, 12);
            correct.Controls.Add(undo);
            undo.Click += Undo_Click;
            

            redo = new Button();
            redo.Text = "redo";
            redo.Size = new Size(80, 30);
            redo.Location = new Point(100, 14);
            redo.Font = new Font(textBox.Font.FontFamily, 12);
            correct.Controls.Add(redo);
            settings.Controls.Add(correct);
            redo.Click += Redo_Click;

            FirstToUp = new CheckBox();
            FirstToUp.Checked = false;
            FirstToUp.Text = "Set the capital first letter in a sentence";
            FirstToUp.Size = new Size(FirstToUp.Text.Length * 12, 30);
            FirstToUp.Location = new Point(Font.Width + alignment.Width + find.Width + correct.Width + 90, 20);
            settings.Controls.Add(FirstToUp);
            FirstToUp.Click += FirstToUp_Click;


            settings.Size = new Size(Width - 30, Height - textBox.Height - 100);
            settings.BackColor = Color.AliceBlue;
            Controls.Add(settings);
            settings.Controls.Add(alignment);

            save = new Button();
            save.Text = "Save";
            save.Size = new Size(80, 30);
            save.Location = new Point(Width/2 - 200, Height - 90);
            save.Font  = new Font(textBox.Font.FontFamily, 12);
            Controls.Add(save);
            save.Click += Save_Click;

            load = new Button();
            load.Text = "Load";
            load.Size = new Size(80, 30);
            load.Location = new Point(Width / 2 + 150, Height - 90);
            load.Font = new Font(textBox.Font.FontFamily, 12);
            Controls.Add(load);
            load.Click += Load_Click;


            this.Resize += Form1_Resize;
        }

        private void BChange_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(search.Text) && !string.IsNullOrEmpty(change.Text))
            {
                textBox.Text = textBox.Text.Replace(search.Text, change.Text);
            }
        }

        private void FirstToUp_Click(object sender, EventArgs e)
        {
            if (FirstToUp.Checked)
            {
                string tmp = "";
                string[] strArr = textBox.Text.Split(".".ToCharArray());
                for (int i =0; i < strArr.Length-1; i++)
                {
                    if (!strArr[i][0].Equals(" "))
                        strArr[i] = strArr[i].Substring(0, 1) + char.ToUpper(strArr[i][1]) + strArr[i].Substring(2) + ".";
                    else
                        strArr[i] = char.ToUpper(strArr[i][0]) + strArr[i].Substring(1) + ".";
                    tmp += strArr[i];
                }
                textBox.Text = tmp;
            }
        }

        private void Undo_Click(object sender, EventArgs e)
        {
            bUndo = true;
            if (MyStack.MainStack != null)
            {
                changes = MyStack.MainStackPull();
                if (changes!=null && !changes.TypeOfChange)
                {
                    textBox.Text = sample.Insert(changes.Position, changes.ChangeText);
                    changes.TypeOfChange = !changes.TypeOfChange;
                    MyStack.BackStackAdd(changes);
                    redo.ForeColor = Color.Blue;
                    redo.BackColor = Color.AliceBlue;
                }
                else if (changes!=null)
                {
                    textBox.Text = sample.Remove(changes.Position, changes.ChangeText.Length);
                    changes.TypeOfChange = !changes.TypeOfChange;
                    MyStack.BackStackAdd(changes);
                    redo.ForeColor = Color.Blue;
                    redo.BackColor = Color.AliceBlue;
                }
            }
            if (MyStack.MainStack.Count == 0)
            {
                undo.ForeColor = Color.Black;
                undo.BackColor = Color.GhostWhite;
            }
            bUndo = false;
        }

        private void Redo_Click(object sender, EventArgs e)
        {
            bRedo = true;
            if (MyStack.BackStack != null) 
            {
                changes = MyStack.BackStackPull();
                if (changes != null && !changes.TypeOfChange)
                {
                    textBox.Text = sample.Insert(changes.Position, changes.ChangeText);
                    MyStack.MainStackAdd(!changes.TypeOfChange, changes.ChangeText, changes.Position);
                    undo.ForeColor = Color.Blue;
                    undo.BackColor = Color.AliceBlue;
                }
                else if (changes!=null)
                {

                    textBox.Text = sample.Remove(changes.Position, changes.ChangeText.Length);
                    MyStack.MainStackAdd(!changes.TypeOfChange, changes.ChangeText, changes.Position);
                    undo.ForeColor = Color.Blue;
                    undo.BackColor = Color.AliceBlue;
                }
            }
            if (MyStack.BackStack.Count == 0)
                {
                    redo.ForeColor = Color.Black;
                    redo.BackColor = Color.GhostWhite;
                }
            bRedo = false;
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (FirstToUp.Checked && textBox.Text.Length == 1 && Convert.ToInt32(textBox.Text[textBox.Text.Length - 1]) > 64)
            {
                textBox.Text = char.ToUpper(textBox.Text[textBox.Text.Length - 1]).ToString();
                textBox.SelectionStart = textBox.Text.Length;
            }
            else if (FirstToUp.Checked && textBox.Text.Length == 1)
                capSymbol = true;
            bool tmp = false;
            if (!string.IsNullOrEmpty(sample) && !bUndo && !bRedo)
            {
                if (textBox.Text.Length > 0)
                {
                    char symb = textBox.Text[textBox.Text.Length - 1];
                    if (FirstToUp.Checked && (symb == '\n' || symb == '.' || symb == '?' || symb == '!'))
                        capSymbol = true;
                    if (Convert.ToInt32(textBox.Text[textBox.Text.Length - 1]) > 64 && capSymbol)
                    {
                        capSymbol = false;
                        string text1 = textBox.Text.Substring(0, textBox.Text.Length - 1);
                        textBox.Text = text1 + char.ToUpper(textBox.Text[textBox.Text.Length - 1]);
                        textBox.SelectionStart = textBox.Text.Length;
                    }
                }
                int length = textBox.Text.Length - sample.Length;
                if (length > 0)
                {
                    for (int i = 0; i < sample.Length; i++)
                    {
                        if (textBox.Text[i] != sample[i])
                        {
                            MyStack.MainStackAdd(true, textBox.Text.Substring(i, length), i);
                            undo.ForeColor = Color.Blue;
                            undo.BackColor = Color.AliceBlue;
                            i = textBox.Text.Length;
                            tmp = true;
                        }
                        if (i == (sample.Length - 1) && !tmp)
                        {
                            MyStack.MainStackAdd(true, textBox.Text.Substring(i+1, length), i+1);
                            undo.ForeColor = Color.Blue;
                            undo.BackColor = Color.AliceBlue;
                        }
                    }
                }
                if (length < 0)
                {
                    for (int i = 0; i < textBox.Text.Length; i++)
                    {
                        if (textBox.Text[i] != sample[i])
                        {
                            MyStack.MainStackAdd(false, sample.Substring(i, length * -1), i);
                            undo.ForeColor = Color.Blue;
                            undo.BackColor = Color.AliceBlue;
                            i = textBox.Text.Length;
                            tmp = true;
                        }
                        if (i == (textBox.Text.Length - 1) && !tmp)
                        {
                            MyStack.MainStackAdd(false, sample.Substring(i + 1, length * -1), i + 1);
                            undo.ForeColor = Color.Blue;
                            undo.BackColor = Color.AliceBlue;
                        }
                    }
                }
            }      
            sample = textBox.Text;      
        }

        private void FontSize_TextChanged(object sender, EventArgs e)
        {
            if (fontSize.Text != "")
            {
                int size = Convert.ToInt32(fontSize.Text);
                if (size > 2)
                    textBox.Font = new Font(textBox.Font.FontFamily, size);
            }
        }

        private void Center_Click(object sender, EventArgs e)
        {
            textBox.TextAlign = HorizontalAlignment.Center;
        }

        private void Left_Click(object sender, EventArgs e)
        {
            textBox.TextAlign = HorizontalAlignment.Left;
        }

        private void Right_Click(object sender, EventArgs e)
        {
            textBox.TextAlign = HorizontalAlignment.Right;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            File.WriteAllText(fileName, textBox.Text);
            MyStack.MainStackClear();
            MyStack.BackStackClear();
            redo.ForeColor = Color.Black;
            redo.BackColor = Color.GhostWhite;
            undo.ForeColor = Color.Black;
            undo.BackColor = Color.GhostWhite;
        }

        private void Load_Click(object sender, EventArgs e)
        {
            if (File.Exists(fileName))
            {
                textBox.Text = File.ReadAllText(fileName);
                sample = textBox.Text;
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            textBox.Width = this.Width - 35;
            textBox.Height = this.Height - 200;
            save.Location = new Point(Width / 2 - 200, Height - 90);
            load.Location = new Point(Width / 2 + 150, Height - 90);
            settings.Size = new Size(Width - 30, Height - textBox.Height - 100);
        }

        CheckBox FirstToUp;

        TextBox textBox;
        TextBox fontSize;
        TextBox search;
        TextBox change;

        Panel alignment;
        RadioButton left;
        RadioButton right;
        RadioButton center;

        Label settings;
        Label Font;
        Label alig;
        Label find;
        Label correct;

        Button bChange;
        Button undo;
        Button redo;

        Button save;
        Button load;
        
    }
}
