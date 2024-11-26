using Avalonia.Controls;
using Avalonia.Media.Imaging;
using AvaloniaApplication1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using static AvaloniaApplication1.SavingDate;
using System.Linq;
using AvaloniaApplication1.Models;
using AvaloniaApplication1;

namespace AvaloniaApplication1
{
    public partial class MainWindow : Window
    {

        private int _SelectedPageIndex; //Выбранный номер страницы
        private int forswitch;
        public List<Клиенты> _AllClients = Helper.user.Клиентыs.Include(x => x.ListTags).ToList(); //Список клиентов из БД (также связанными данными заполнены коллекции в объектах: теги, посещения, файлы)

        public MainWindow()
        {
            InitializeComponent();
            Listins(_AllClients);
            //Listins(SavingDate.klient.OrderByDescending(x => x.Id));
            FiltrPol.SelectedIndex = 0;
            Filtr.SelectedIndex = 0;
            cbox_display.SelectedIndex = 0;

        }

        private void Listins(List<Клиенты> list)
        {

            ListPers.ItemsSource = list.Select(x => new
            {

                x.Id,
                x.Name,
                Gender = x.Gender == 1 ? "Мужчина" : "Женщина",
                x.Surname,
                x.MiddleName,
                x.Birthday,
                x.DateRegistr,
                x.NumberPhone,
                x.EmailAdress,

                x.ColvoVisit,
                photo = new Bitmap($"Assets/{x.Photo}"),

                Tagsss = x.ListTags.Select(y => new
                {
                    y.IdTag,



                    tag[(int)y.IdTag - 1].Name,
                    Color = $"{tag[(int)y.IdTag - 1].Color}",
                    //tag[(int)y.IdTag - 1].Color,
                }),


            });
            ColvoText.Text = $"Выведено записей {SavingDate.klient.Count} из {SavingDate.klient.Count}";
        }


        private void Filtrs(List<Клиенты> clients)
        {
            List<Клиенты> dsf = clients;
            ClientsDisplayed(dsf, forswitch);
            int d = Filtr.SelectedIndex;
            if (d == 1)
            {
                dsf = dsf.OrderBy(x => x.MiddleName).ToList();
                Listins(dsf);

            }
            else if (d == 2)
            {
                dsf = dsf.OrderByDescending(x => x.ColvoVisit).ToList();
                Listins(dsf);
            }

            int v = FiltrPol.SelectedIndex;
            if (v == 1)
            {
                Listins(dsf.Where(x => x.Gender == 1).ToList()); ColvoText.Text = $"Выведено записей {SavingDate.klient.Where(x => x.Gender == 1).Count()} из {SavingDate.klient.Count}";
            }
            else if (v == 2)
            {
                Listins(dsf.Where(x => x.Gender == 2).ToList()); ColvoText.Text = $"Выведено записей {SavingDate.klient.Where(x => x.Gender == 2).Count()} из {SavingDate.klient.Count}";
            }

        }
        private void Filtrs1()
        {
            int d = Filtr.SelectedIndex;
            switch (d)
            {
                case 1: Listins(SavingDate.klient.OrderBy(x => x.MiddleName).ToList()); break;
                case 2: Listins(SavingDate.klient.OrderByDescending(x => x.ColvoVisit).ToList()); break;
            }
        }

        private void ComboBox_SizeChanged(object? sender, Avalonia.Controls.SizeChangedEventArgs e)
        {
            Filtrs(SavingDate.klient);


        }

        private void ComboBox_SizeChanged_2(object? sender, Avalonia.Controls.SizeChangedEventArgs e)
        {
            Filtrs(SavingDate.klient);

        }
        private void ClientsDisplayed(List<Клиенты> clients, int forswitch)
        {
            SavingDate._ClientsPages.Clear();
            int displayedClientsCount = 1;
            switch (forswitch)
            {
                case 1:
                    displayedClientsCount = 10;
                    break;
                case 2:
                    displayedClientsCount = 50;
                    break;
                case 3:
                    displayedClientsCount = 200;
                    break;
                default:
                    Listins(clients);
                    return;
            }
            displayedClientsCount = displayedClientsCount > clients.Count ? clients.Count : displayedClientsCount;
            int listCount = (int)Math.Ceiling((double)clients.Count / displayedClientsCount);
            int l = 0; //Счетчик для всех клиентов
            for (int j = 0; j < listCount; j++)
            {
                List<Клиенты> displayedClients = [];
                int testint = (displayedClientsCount > clients.Count - displayedClientsCount * j ? clients.Count - displayedClientsCount * j : displayedClientsCount);
                for (int i = 0; i < testint; i++)
                {
                    displayedClients.Add(clients[l]);
                    l++;
                }
                SavingDate._ClientsPages.Add(displayedClients);
            }
            _SelectedPageIndex = 0;
            Listins(clients.Count > 0 ? SavingDate._ClientsPages[_SelectedPageIndex] : clients);
            PageTextDisplay(displayedClientsCount);
        }

        private void ComboBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
        {
            int v = cbox_display.SelectedIndex;
            ClientsDisplayed(SavingDate.klient, v);
        }
        private void PageTextDisplay(int displayedClientsCount) //отображение номера страницы
        {
            int v = displayedClientsCount;
            if (SavingDate._ClientsPages.Count > 0)
            {
                tblock_page.IsVisible = true;
                tblock_pageCount.IsVisible = true;
                tblock_pageCount.Text = $"{_SelectedPageIndex + 1}/{SavingDate._ClientsPages.Count}";
                ColvoText.Text = $"Выведено записей {v} из {SavingDate.klient.Count}";
            }
            else
            {
                tblock_page.IsVisible = tblock_pageCount.IsVisible = false;
            }
        }
        private void Stranich(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (cbox_display.SelectedIndex != 0)
            {
                int displayedClientsCount = 0;
                switch (forswitch)
                {
                    case 1:
                        displayedClientsCount = 10;
                        break;
                    case 2:
                        displayedClientsCount = 50;
                        break;
                    case 3:
                        displayedClientsCount = 200;
                        break;
                }
                var btn = (sender as Button)!;
                switch (btn.Name)
                {
                    case "btn_nazad":
                        _SelectedPageIndex--;
                        if (_SelectedPageIndex >= 0)
                        {

                            PageTextDisplay(displayedClientsCount);
                            Listins(SavingDate._ClientsPages[_SelectedPageIndex]);
                        }
                        else
                        {
                            _SelectedPageIndex++;
                        }
                        break;
                    case "btn_next":
                        _SelectedPageIndex++;
                        if (_SelectedPageIndex < SavingDate._ClientsPages.Count)
                        {
                            PageTextDisplay(displayedClientsCount);
                            Listins(SavingDate._ClientsPages[_SelectedPageIndex]);
                        }
                        else
                        {
                            _SelectedPageIndex--;
                        }
                        break;
                }
            }
        }

        private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            new Redactor().Show();
            Close();
        }

        private void Button_Click_2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {

            var btn = (sender as Button)!;
            switch (btn.Name)
            {
                case "btn_red":
                    int v = ((int)btn!.Tag!);
                    SavingDate._RedClient = SavingDate.klient[v - 2];//((int)btn!.Tag!)
                    break;
            }

            new Redactor().Show(); Close();

        }

        private void Button_Click_3(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var btn = (sender as Button)!;
            switch (btn.Name)
            {
                case "History":
                    int v = ((int)btn!.Tag!);
                    SavingDate._RedClient = SavingDate.klient[v - 2];//((int)btn!.Tag!)
                    break;
            }

            new HistoryAndFile().Show(); Close();
        }
    }
}