using System.Diagnostics;
public class Frame : Form
{
    
    static Button[,] buttons = new Button[3,3];
    
    public Frame()
    {
        Icon = Icon.ExtractAssociatedIcon(Path.GetDirectoryName(stackFrame.GetFileName()) + "\\Images\\TicTacToeIcon.ico");
        Text = "Tic Tac Toe";
        Size = new Size(600, 600);
        BackColor = Color.White;
        
        TableLayoutPanel tbl = new();
        tbl.Dock = DockStyle.Fill;
        tbl.RowCount = 3;
        tbl.ColumnCount = 3;
        for (int i = 0; i < 3; i++)
        {
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333f));
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                buttons[i, j] = new RoundedButton();
                buttons[i, j].Dock = DockStyle.Fill;
                buttons[i, j].BackgroundImageLayout = ImageLayout.Zoom;
                buttons[i, j].BackColor = Color.LightGray;
                
                tbl.Controls.Add(buttons[i,j], i, j);
                buttons[i, j].Click += ButtonListener;
            }
        }
        
        Controls.Add(tbl);
    }

    private void ButtonListener(object sender, EventArgs e)
    {
        var clickedButton = sender as Button;

        for (int i = 0; i < 9; i++) 
            if (clickedButton == buttons[i / 3, i % 3]) SetButtonState(i);

    }

    public static void SetButtonImage(int pos, Image stateImage)
    {
        buttons[pos / 3, pos % 3].BackgroundImage = stateImage;
        buttons[pos / 3, pos % 3].Enabled = false;
        buttons[pos / 3, pos % 3].BackColor = Color.DarkGray;
    }

    public static void GameEndRefresh(int[] combNums)
    {
        for (int i = 0; i < 3; i++)
            buttons[(combNums[i] - 1) / 3, (combNums[i] - 1) % 3].BackColor = Color.LimeGreen;
        
        for (int i = 0; i < 9; i++)
            buttons[i / 3, i % 3].Enabled = false;
    }
    private void GameEnd(string result)
    {
        if (MessageBox.Show(result,"The Game Ended",MessageBoxButtons.RetryCancel, MessageBoxIcon.None, MessageBoxDefaultButton.Button1).Equals(DialogResult.Retry))
        {
            Frame frame = new Frame();
            frame.Show();
            Hide();
        }
        else Environment.Exit(0);
    }
    
    public static StackFrame stackFrame = new StackTrace(new StackFrame(true)).GetFrame(0);
    private Image X = Image.FromFile(Path.GetDirectoryName(stackFrame.GetFileName()) + "\\Images\\X.png");
    private Image O = Image.FromFile(Path.GetDirectoryName(stackFrame.GetFileName()) + "\\Images\\O.png");
    
    private string _occupiedByX = "000000000", _occupiedByO = "000000000";
    
    private int[,] combinations = {{1,2,3}, {4,5,6}, {7,8,9}, {1,4,7}, {2,5,8}, {3,6,9}, {1,5,9}, {3,5,7}}; 

    private int _n = 0;
    
    private string Update(string str, int pos, Image stateImage)
    {
        SetButtonImage(pos, stateImage);
        return str.Substring(0, pos) + "1" + str.Substring(pos + 1);
    }

    private void CombinationCheck(string str)
    {
        for (int i = 0; i < 8; i++)
            if (str.Substring(combinations[i, 0] - 1, 1).Equals("1") &&
                str.Substring(combinations[i, 1] - 1, 1).Equals("1") &&
                str.Substring(combinations[i, 2] - 1, 1).Equals("1"))
            {
                Frame.GameEndRefresh(new[] { combinations[i, 0], combinations[i, 1], combinations[i, 2] });
                
                if(_n % 2 == 0) GameEnd("X Won!");
                else GameEnd("O Won!");
            }
    }

    public void SetButtonState(int pressedButton)
    {
        
        if (_occupiedByX.Substring(pressedButton, 1) != "1" && _occupiedByO.Substring(pressedButton, 1) != "1")
        {
            if (_n % 2 == 0)
            {
                _occupiedByX = Update(_occupiedByX, pressedButton, X);
                CombinationCheck(_occupiedByX);
            }
            else
            {
                _occupiedByO = Update(_occupiedByO, pressedButton, O);
                CombinationCheck(_occupiedByO);
            }   
        }

        _n++;
        
        if(_n == 9) GameEnd("Tie!");
    }
        
}

static class Program
{
    static void Main()
    {
        Frame frame = new Frame();
        Application.Run(frame);
    }
}