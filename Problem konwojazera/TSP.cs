using System;
using System.Drawing;
using System.Windows.Forms;

namespace TravellingSalesmanProblem
{
    public partial class TSP : Form
    {
        private TextBox inputTextBox;
        private Button solveButton;
        private Label resultLabel;

        private int numberOfNodes, numberOfEdges, startNode, minCycleWeight, currentPathWeight, solutionPointer, currentPathPointer;
        private bool[,] adjacencyMatrix; 
        private int[,] weightMatrix;     
        private int[] solutionPath, currentPath; 
        private bool[] visitedNodes;   

        public TSP()
        {
            InitializeComponent();
            SetupControls();
        }

        private void SetupControls()
        {
            this.Text = "Travelling Salesman Problem Solver";
            this.Size = new Size(400, 400);

            inputTextBox = new TextBox
            {
                Multiline = true,
                Width = 350,
                Height = 100,
                Top = 10,
                Left = 20,
                Font = new Font("Arial", 10),
                PlaceholderText = "Enter the graph data here..."
            };

            solveButton = new Button
            {
                Text = "Solve TSP",
                Top = 120,
                Left = 20,
                Width = 350,
                Height = 40,
                Font = new Font("Arial", 12),
                BackColor = Color.LightBlue
            };

            resultLabel = new Label
            {
                Text = "Result will appear here",
                Top = 170,
                Left = 20,
                Width = 350,
                Height = 200,
                Font = new Font("Arial", 10),
                BorderStyle = BorderStyle.FixedSingle
            };

            solveButton.Click += SolveButton_Click;

            this.Controls.Add(inputTextBox);
            this.Controls.Add(solveButton);
            this.Controls.Add(resultLabel);
        }

        private void SolveButton_Click(object sender, EventArgs e)
        {
            try
            {
                string[] lines = inputTextBox.Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                numberOfNodes = int.Parse(lines[0].Split()[0]);
                numberOfEdges = int.Parse(lines[0].Split()[1]);
                solutionPath = new int[numberOfNodes];
                currentPath = new int[numberOfNodes];
                visitedNodes = new bool[numberOfNodes];
                adjacencyMatrix = new bool[numberOfNodes, numberOfNodes];
                weightMatrix = new int[numberOfNodes, numberOfNodes];

                currentPathWeight = 0;
                minCycleWeight = int.MaxValue;
                solutionPointer = 0;
                currentPathPointer = 0;
                Array.Clear(visitedNodes, 0, visitedNodes.Length);

                for (int i = 1; i <= numberOfEdges; i++)
                {
                    string[] parts = lines[i].Split();
                    int x = int.Parse(parts[0]);
                    int y = int.Parse(parts[1]);
                    int z = int.Parse(parts[2]);
                    adjacencyMatrix[x, y] = adjacencyMatrix[y, x] = true;
                    weightMatrix[x, y] = weightMatrix[y, x] = z;
                }

                startNode = 0;
                SolveTSP(startNode);
                if (solutionPointer > 0)
                {
                    string path = "";
                    for (int i = 0; i < solutionPointer; i++)
                        path += solutionPath[i] + " ";
                    path += startNode;
                    resultLabel.Text = "Path: " + path + "\nMinimum weight: " + minCycleWeight;
                }
                else
                {
                    resultLabel.Text = "NO HAMILTONIAN CYCLE";
                }
            }
            catch (Exception ex)
            {
                resultLabel.Text = "Error: " + ex.Message;
            }
        }

        private void SolveTSP(int node)
        {
            currentPath[currentPathPointer++] = node;

            if (currentPathPointer < numberOfNodes)
            {
                visitedNodes[node] = true;
                for (int nextNode = 0; nextNode < numberOfNodes; nextNode++)
                {
                    if (adjacencyMatrix[node, nextNode] && !visitedNodes[nextNode])
                    {
                        currentPathWeight += weightMatrix[node, nextNode];
                        SolveTSP(nextNode);
                        currentPathWeight -= weightMatrix[node, nextNode];
                    }
                }
                visitedNodes[node] = false;
            }
            else if (adjacencyMatrix[startNode, node])
            {
                currentPathWeight += weightMatrix[node, startNode];
                if (currentPathWeight < minCycleWeight)
                {
                    minCycleWeight = currentPathWeight;
                    for (int i = 0; i < currentPathPointer; i++)
                        solutionPath[i] = currentPath[i];
                    solutionPointer = currentPathPointer;
                }
                currentPathWeight -= weightMatrix[node, startNode];
            }
            currentPathPointer--;
        }
    }
}
