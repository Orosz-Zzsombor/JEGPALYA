using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace jegpalya
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Random random = new Random();
        public MainWindow()
        {
            InitializeComponent();

            JegpalyaLetrehozasa();
            Feltolt();
        }
        int Oszlopok = 10;
        int Sorok = 10;
        public void JegpalyaLetrehozasa()
        {

            for (int i = 0; i < Oszlopok; i++)
            {
                ColumnDefinition ujOszlop = new();
                grdJegpalya.ColumnDefinitions.Add(ujOszlop);

            }
            for (int y = 0; y < Sorok; y++)
            {
                RowDefinition ujSor = new();
                grdJegpalya.RowDefinitions.Add(ujSor);
            }


        }

        public void Feltolt()
        {
            int babukSzama = 20;
            int lyukakSzama = 10;

            List<(int row, int col)> positions = new List<(int row, int col)>();

            // Initialize all positions in the grid
            for (int row = 0; row < Sorok; row++)
            {
                for (int col = 0; col < Oszlopok; col++)
                {
                    positions.Add((row, col));
                }
            }


            Shuffle(positions);

            // Place buttons
            for (int i = 0; i < babukSzama; i++)
            {
                var (row, col) = positions[i];
                Button button = new Button
                {
                    Content = i
                };
                button.Background = Brushes.Yellow;
                Grid.SetRow(button, row);
                Grid.SetColumn(button, col);
                grdJegpalya.Children.Add(button);
            }

            // Place text blocks
            for (int i = babukSzama; i < babukSzama + lyukakSzama; i++)
            {
                var (row, col) = positions[i];
                TextBlock textBlock = new TextBlock
                {
                    Foreground = Brushes.White,
                };
                textBlock.Background = Brushes.DarkBlue;
                Grid.SetRow(textBlock, row);
                Grid.SetColumn(textBlock, col);
                grdJegpalya.Children.Add(textBlock);
            }
        }

        private void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private void btnLeptet_Click(object sender, RoutedEventArgs e)
        {
            List<Button> buttons = new List<Button>();

            // Collect all buttons in the grid
            foreach (UIElement element in grdJegpalya.Children)
            {
                if (element is Button button)
                {
                    buttons.Add(button);
                }
            }

            // Move each button to a nearby empty cell
            foreach (Button button in buttons)
            {
                int currentRow = Grid.GetRow(button);
                int currentCol = Grid.GetColumn(button);

                (int newRow, int newCol)? newPosition = GetNearbyEmptyCell(currentRow, currentCol);

                if (newPosition.HasValue)
                {
                    (int newRow, int newCol) = newPosition.Value;
                    Grid.SetRow(button, newRow);
                    Grid.SetColumn(button, newCol);
                }
            }
        }

        private (int row, int col)? GetNearbyEmptyCell(int currentRow, int currentCol)
        {
            // Define possible moves (up, down, left, right)
            var possibleMoves = new List<(int row, int col)>
            {
                (currentRow - 1, currentCol),
                (currentRow + 1, currentCol),
                (currentRow, currentCol - 1),
                (currentRow, currentCol + 1)
            };

            // Filter out moves that are out of grid bounds or occupied
            var validMoves = possibleMoves
                .Where(pos => pos.row >= 0 && pos.row < Sorok && pos.col >= 0 && pos.col < Oszlopok)
                .Where(pos => !IsCellOccupied(pos.row, pos.col))
                .ToList();

            if (validMoves.Count > 0)
            {
                // Return a random valid move
                return validMoves[random.Next(validMoves.Count)];
            }
            else
            {
                // No valid moves available
                return null;
            }
        }

        private bool IsCellOccupied(int row, int col)
        {
            foreach (UIElement child in grdJegpalya.Children)
            {
                if (Grid.GetRow(child) == row && Grid.GetColumn(child) == col)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

    


