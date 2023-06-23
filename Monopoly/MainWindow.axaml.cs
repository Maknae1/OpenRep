using Avalonia.Controls;
using Monopoly.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Monopoly
{
    public partial class MainWindow : Window
    {
        int selectedPalleteId = 0;
        public MainWindow()
        {
            InitializeComponent();
            LoadBoxes();
            LoadPalletes();
            LoadData();
            //palleteListBox.SelectionChanged += PalleteListBoxSelectionChanged;
            ExpiretionComboBox.SelectionChanged += ExpiretionComboBoxSelectionChanged;
            WeightComboBox.SelectionChanged += WeightComboBoxSelectionChanged;
            threePalletsButton.Click += ThreePalletsButtonClick;
        }

        private void ThreePalletsButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            List<Pallets> pallets = ListClass.PalletsList.OrderByDescending(f => f.Volume).Take(3).ToList();

            //����� ������ ������
            palleteListBox.Items = pallets.OrderBy(f => f.Volume).Select(f => new
            {
                PalleteID = $"����� �������: {f.ID}",
                Width = $"������: {f.Width} �",
                Height = $"������: {f.Height} �",
                Depth = $"�������: {f.Depth} �",
                ExpirationDate = $"���� �������� ��������: {f.ExpirationDate}",
                Weight = $"���: {f.Weight} ��",
                Volume = $"�����: {f.Volume}  �3",
                f.ID
            });

        }

        private void ShowItemsButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            selectedPalleteId = (int)(sender as Button).Tag;

            List<Boxes> boxes = ListClass.BoxesList;
            //����� ������ �������
            if (selectedPalleteId != 0)
            {
                boxes = boxes.Where(f => f.PalleteID == selectedPalleteId).ToList();
                emptyListTextBox.Text = "���������� �������";

                boxesListBox.Items = boxes.Select(f => new
                {
                    ID = $"����� �������: {f.ID}",
                    Width = $"������: {f.Width} ��",
                    Height = $"������: {f.Height} ��",
                    Depth = $"�������: {f.Depth} ��",
                    ManufactureDate = f.ManufactureDate != new DateTime(0001, 1, 1) ? $"���� ������������: {DateOnly.FromDateTime(f.ManufactureDate)}" : "���� ������������: ???",
                    ExpirationDate = $"���� �������� ��������: {f.ExpirationDate}",
                    Weight = $"���: {f.Weight} ��",
                    Volume = $"�����: {f.Volume}  �3",
                });
            }
            else
            {
                emptyListTextBox.Text = "�������� �������";
            }

        }

        private void WeightComboBoxSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            LoadData();
        }

        private void ExpiretionComboBoxSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            LoadData();
        }

        public void LoadData()
        {
            boxesListBox.Items = null;
            List<Pallets> pallets = ListClass.PalletsList;

            //����������� �� ����� �������� � ���������� �� ����
            switch (ExpiretionComboBox.SelectedIndex)
            {
                case 0:
                    if(WeightComboBox.SelectedIndex == 0)
                        pallets = pallets.Where(f => f.ExpirationDate.Month == DateOnly.FromDateTime(DateTime.Today).Month).OrderBy(f => f.Weight).ToList();
                    else
                    {
                        pallets = pallets.Where(f => f.ExpirationDate.Month == DateOnly.FromDateTime(DateTime.Today).Month).OrderByDescending(f => f.Weight).ToList();
                    }
                    break;
                case 1:
                    if (WeightComboBox.SelectedIndex == 0)
                        pallets = pallets.Where(f => f.ExpirationDate.Month == DateOnly.FromDateTime(DateTime.Today.AddMonths(1)).Month).OrderBy(f => f.Weight).ToList();
                    else
                    {
                        pallets = pallets.Where(f => f.ExpirationDate.Month == DateOnly.FromDateTime(DateTime.Today.AddMonths(1)).Month).OrderByDescending(f => f.Weight).ToList();
                    }
                    break;
                case 2:
                    if (WeightComboBox.SelectedIndex == 0)
                        pallets = pallets.Where(f => f.ExpirationDate.Month >= DateOnly.FromDateTime(DateTime.Today.AddMonths(2)).Month || f.ExpirationDate.Year > DateOnly.FromDateTime(DateTime.Today).Year).OrderBy(f => f.Weight).ToList();
                    else
                    {
                        pallets = pallets.Where(f => f.ExpirationDate.Month >= DateOnly.FromDateTime(DateTime.Today.AddMonths(2)).Month).OrderByDescending(f => f.Weight).ToList();
                    }
                    break;
                case 3:
                    if (WeightComboBox.SelectedIndex == 0)
                        pallets = pallets.OrderBy(f => f.Weight).ToList();
                    else
                    {
                        pallets = pallets.OrderByDescending(f => f.Weight).ToList();
                    }
                    break;
            }

            //�������� ����� � �������������, ���� ��� ���������, ��������������� ���������
            if(pallets.Count == 0)
            {
                palleteTextblock.FontWeight = Avalonia.Media.FontWeight.DemiBold;
                emptyListTextBox.IsVisible = false;
                boxesBorder.IsVisible = false;
                palleteTextblock.Text = "������ �� �������";
            }
            else
            {
                emptyListTextBox.IsVisible = true;
                boxesBorder.IsVisible = true;
                palleteTextblock.FontWeight = Avalonia.Media.FontWeight.Regular;
                palleteTextblock.Text = "������ ������";
            }
            

            //����� ������ ������
            palleteListBox.Items = pallets.Select(f => new
            {
                PalleteID = $"����� �������: {f.ID}",
                Width = $"������: {f.Width} �",
                Height = $"������: {f.Height} �",
                Depth = $"�������: {f.Depth} �",
                ExpirationDate = $"���� �������� ��������: {f.ExpirationDate}",
                Weight = $"���: {f.Weight} ��",
                Volume = $"�����: {f.Volume}  �3",
                f.ID
            });

            
        }

        public void LoadBoxes()
        {
            Random random = new Random();
            ListClass.BoxesList = new List<Boxes>()
            {
                new Boxes { ID = 1, PalleteID = 1, Width = random.Next(10,91), Height = random.Next(10,91), Depth = random.Next(10, 91), Weight = random.Next(10,91), ManufactureDate = new DateTime(2023, random.Next(6, 12), random.Next(1,29))},
                new Boxes { ID = 2, PalleteID = 1, Width = random.Next(10,91), Height = random.Next(10,91), Depth = random.Next(10,91), Weight = random.Next(10,91) },
                new Boxes { ID = 3, PalleteID = 1, Width = random.Next(10,91), Height = random.Next(10,91), Depth = random.Next(10,91), Weight = random.Next(10,91), ManufactureDate = new DateTime(2023, random.Next(6, 12), random.Next(1,29)) },
                new Boxes { ID = 4, PalleteID = 2, Width = random.Next(10,91), Height = random.Next(10,91), Depth = random.Next(10,91), Weight = random.Next(10,91), ManufactureDate = new DateTime(2023, random.Next(6, 12), random.Next(1,29)) },
                new Boxes { ID = 5, PalleteID = 2, Width = random.Next(10,91), Height = random.Next(10,91), Depth = random.Next(10,91), Weight = random.Next(10,91), ManufactureDate = new DateTime(2023, random.Next(6, 12), random.Next(1,29)) },
                new Boxes { ID = 6, PalleteID = 2, Width = random.Next(10,91), Height = random.Next(10,91), Depth = random.Next(10,91), Weight = random.Next(10,91), ManufactureDate = new DateTime(2023, 4, random.Next(1,29)) },
                new Boxes { ID = 7, PalleteID = 3, Width = random.Next(10,91), Height = random.Next(10,91), Depth = random.Next(10,91), Weight = random.Next(10,91), ManufactureDate = new DateTime(2023, random.Next(6, 12), random.Next(1,29)) },
                new Boxes { ID = 8, PalleteID = 3, Width = random.Next(10,91), Height = random.Next(10,91), Depth = random.Next(10,91), Weight = random.Next(10,91), ManufactureDate = new DateTime(2023, random.Next(6, 12), random.Next(1,29)) },
                new Boxes { ID = 9, PalleteID = 3, Width = random.Next(10,91), Height = random.Next(10,91), Depth = random.Next(10,91), Weight = random.Next(10,91), ManufactureDate = new DateTime(2023, random.Next(6, 12), random.Next(1,29)) },
                new Boxes { ID = 10, PalleteID = 4, Width = random.Next(10,91), Height = random.Next(10,91), Depth = random.Next(10,91), Weight = random.Next(10,91), ManufactureDate = new DateTime(2023, random.Next(6, 12), random.Next(1,29)) }
            };

        }

        public void LoadPalletes()
        {
            ListClass.PalletsList = new List<Pallets>()
            {
                new Pallets { ID = 1, Width = 5, Height = 5, Depth = 5 },
                new Pallets { ID = 2, Width = 5, Height = 5, Depth = 5 },
                new Pallets { ID = 3, Width = 5, Height = 5, Depth = 5 },
                new Pallets { ID = 4, Width = 5, Height = 5, Depth = 5 }
            };

        }
    }
}