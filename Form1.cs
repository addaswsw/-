using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace 作业1
{
    public partial class Form1 : Form
    {
        private ComboBox comboBoxDifficulty;
        private Button buttonGenerate;
        private Button buttonSubmit;
        private Button buttonRegenerate;
        private Panel panelQuestions;
        private Label labelTitle;
        private Label labelResult;

        private List<Question> questions = new();
        private List<GroupBox> questionBoxes = new();
        private readonly string[] difficulties = { "简单", "中等", "困难" };

        public Form1()
        {
            InitializeComponent();
            InitializeCustomUI();
            GenerateQuestions();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 在窗体加载时执行的代码
        }

        private void InitializeCustomUI()
        {
            // 基础窗体设置
            this.Text = "数学单选题";
            this.Size = new Size(1100, 1100);
            this.BackColor = Color.WhiteSmoke;
            this.Font = new Font("微软雅黑", 12);

            // 标题
            labelTitle = new Label
            {
                Text = "数学单选题",
                Font = new Font("微软雅黑", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(450, 40)
            };
            this.Controls.Add(labelTitle);

            // 难度选择
            comboBoxDifficulty = new ComboBox
            {
                Location = new Point(120, 120),
                Size = new Size(160, 36),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("微软雅黑", 13)
            };
            comboBoxDifficulty.Items.AddRange(difficulties);
            comboBoxDifficulty.SelectedIndex = 0;
            this.Controls.Add(comboBoxDifficulty);

            // 生成题目按钮
            buttonGenerate = new Button
            {
                Text = "生成题目",
                Location = new Point(320, 120),
                Size = new Size(220, 65), // 更大按钮
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("微软雅黑", 13) // 字体调小
            };
            buttonGenerate.FlatAppearance.BorderSize = 0;
            buttonGenerate.Click += (s, e) => GenerateQuestions();
            this.Controls.Add(buttonGenerate);

            // 提交按钮
            buttonSubmit = new Button
            {
                Text = "提交答案",
                Location = new Point(570, 120),
                Size = new Size(220, 65), // 更大按钮
                BackColor = Color.FromArgb(39, 174, 96),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("微软雅黑", 13) // 字体调小
            };
            buttonSubmit.FlatAppearance.BorderSize = 0;
            buttonSubmit.Click += (s, e) => SubmitAnswers();
            this.Controls.Add(buttonSubmit);

            // 重新生成按钮
            buttonRegenerate = new Button
            {
                Text = "重新生成",
                Location = new Point(820, 120),
                Size = new Size(220, 65), // 更大按钮
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("微软雅黑", 13) // 字体调小
            };
            buttonRegenerate.FlatAppearance.BorderSize = 0;
            buttonRegenerate.Click += (s, e) => GenerateQuestions();
            this.Controls.Add(buttonRegenerate);

            // 题目面板
            panelQuestions = new Panel
            {
                Location = new Point(80, 200),
                Size = new Size(900, 750),
                AutoScroll = true,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(panelQuestions);

            // 结果标签
            labelResult = new Label
            {
                Text = "",
                Font = new Font("微软雅黑", 15, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                AutoSize = true,
                Location = new Point(80, 980)
            };
            this.Controls.Add(labelResult);
        }

        private void GenerateQuestions()
        {
            panelQuestions.Controls.Clear();
            questionBoxes.Clear();
            questions = GenerateRandomQuestions(comboBoxDifficulty.SelectedIndex, 5);

            int y = 30; // 增大首题距离面板顶部的间隔
            for (int i = 0; i < questions.Count; i++)
            {
                var q = questions[i];
                var groupBox = new GroupBox
                {
                    Text = $"第{i + 1}题",
                    Location = new Point(20, y),
                    Size = new Size(850, 110), // 增大每题高度
                    BackColor = Color.White
                };

                var label = new Label
                {
                    Text = q.Text,
                    Location = new Point(20, 35), // 题干下移
                    AutoSize = true,
                    Font = new Font("微软雅黑", 13, FontStyle.Bold)
                };
                groupBox.Controls.Add(label);

                for (int j = 0; j < 4; j++)
                {
                    var radio = new RadioButton
                    {
                        Text = q.Options[j],
                        Location = new Point(300 + j * 150, 40), // 选项整体下移并右移
                        Tag = j,
                        AutoSize = true,
                        Font = new Font("微软雅黑", 12)
                    };
                    groupBox.Controls.Add(radio);
                }

                panelQuestions.Controls.Add(groupBox);
                questionBoxes.Add(groupBox);
                y += 130; // 增大每题之间的间隔
            }
            labelResult.Text = "";
        }

        private void SubmitAnswers()
        {
            int correct = 0;
            for (int i = 0; i < questions.Count; i++)
            {
                var groupBox = questionBoxes[i];
                var selected = groupBox.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
                if (selected != null && selected.Tag != null && selected.Tag.ToString() == questions[i].CorrectOption.ToString())
                {
                    correct++;
                    groupBox.BackColor = Color.FromArgb(200, 255, 245, 200); // 浅绿色
                }
                else
                {
                    groupBox.BackColor = Color.FromArgb(200, 255, 200, 200); // 浅红色
                }
            }
            labelResult.Text = $"得分：{correct} / {questions.Count}";
        }

        private List<Question> GenerateRandomQuestions(int difficulty, int count)
        {
            var list = new List<Question>();
            var rand = new Random();
            for (int i = 0; i < count; i++)
            {
                int a = 0, b = 0, answer = 0;
                string op = "+";
                switch (difficulty)
                {
                    case 0: // 简单
                        a = rand.Next(1, 21);
                        b = rand.Next(1, 21);
                        op = rand.Next(2) == 0 ? "+" : "-";
                        answer = op == "+" ? a + b : a - b;
                        break;
                    case 1: // 中等
                        a = rand.Next(10, 51);
                        b = rand.Next(10, 51);
                        op = rand.Next(2) == 0 ? "+" : "-";
                        answer = op == "+" ? a + b : a - b;
                        break;
                    case 2: // 困难
                        a = rand.Next(10, 101);
                        b = rand.Next(2, 21);
                        op = rand.Next(2) == 0 ? "*" : "/";
                        if (op == "*")
                        {
                            answer = a * b;
                        }
                        else
                        {
                            answer = a / b;
                            a = answer * b; // 保证整除
                        }
                        break;
                }
                string text = $"{a} {op} {b} = ?";
                var options = new List<string>();
                int correctIndex = rand.Next(4);
                var used = new HashSet<int> { answer };
                for (int j = 0; j < 4; j++)
                {
                    if (j == correctIndex)
                        options.Add(answer.ToString());
                    else
                    {
                        int wrong;
                        do
                        {
                            wrong = answer + rand.Next(-10, 11);
                        } while (used.Contains(wrong));
                        used.Add(wrong);
                        options.Add(wrong.ToString());
                    }
                }
                list.Add(new Question
                {
                    Text = text,
                    Options = options.ToArray(),
                    CorrectOption = correctIndex
                });
            }
            return list;
        }
    }

    public class Question
    {
        public string Text { get; set; }
        public string[] Options { get; set; }
        public int CorrectOption { get; set; }
    }
}