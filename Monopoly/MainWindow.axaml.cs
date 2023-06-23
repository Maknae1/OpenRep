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

            //вывод списка паллет
            palleteListBox.Items = pallets.OrderBy(f => f.Volume).Select(f => new
            {
                PalleteID = $"Номер паллеты: {f.ID}",
                Width = $"Ширина: {f.Width} м",
                Height = $"Высота: {f.Height} м",
                Depth = $"Глубина: {f.Depth} м",
                ExpirationDate = $"Срок годности истекает: {f.ExpirationDate}",
                Weight = $"Вес: {f.Weight} кг",
                Volume = $"Объём: {f.Volume}  м3",
                f.ID
            });

        }

        private void ShowItemsButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            selectedPalleteId = (int)(sender as Button).Tag;

            List<Boxes> boxes = ListClass.BoxesList;
            //вывод списка коробок
            if (selectedPalleteId != 0)
            {
                boxes = boxes.Where(f => f.PalleteID == selectedPalleteId).ToList();
                emptyListTextBox.Text = "Содержание паллеты";

                boxesListBox.Items = boxes.Select(f => new
                {
                    ID = $"Номер коробки: {f.ID}",
                    Width = $"Ширина: {f.Width} см",
                    Height = $"Высота: {f.Height} см",
                    Depth = $"Глубина: {f.Depth} см",
                    ManufactureDate = f.ManufactureDate != new DateTime(0001, 1, 1) ? $"Дата производства: {DateOnly.FromDateTime(f.ManufactureDate)}" : "Дата производства: ???",
                    ExpirationDate = $"Срок годности истекает: {f.ExpirationDate}",
                    Weight = $"Вес: {f.Weight} кг",
                    Volume = $"Объём: {f.Volume}  м3",
                });
            }
            else
            {
                emptyListTextBox.Text = "Выберите паллету";
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

            //группировка по сроку годности и фильтрация по весу
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

            //обратная связь с пользователем, если нет элементов, удовлетворяющих критериям
            if(pallets.Count == 0)
            {
                palleteTextblock.FontWeight = Avalonia.Media.FontWeight.DemiBold;
                emptyListTextBox.IsVisible = false;
                boxesBorder.IsVisible = false;
                palleteTextblock.Text = "Ничего не найдено";
            }
            else
            {
                emptyListTextBox.IsVisible = true;
                boxesBorder.IsVisible = true;
                palleteTextblock.FontWeight = Avalonia.Media.FontWeight.Regular;
                palleteTextblock.Text = "Список паллет";
            }
            

            //вывод списка паллет
            palleteListBox.Items = pallets.Select(f => new
            {
                PalleteID = $"Номер паллеты: {f.ID}",
                Width = $"Ширина: {f.Width} м",
                Height = $"Высота: {f.Height} м",
                Depth = $"Глубина: {f.Depth} м",
                ExpirationDate = $"Срок годности истекает: {f.ExpirationDate}",
                Weight = $"Вес: {f.Weight} кг",
                Volume = $"Объём: {f.Volume}  м3",
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