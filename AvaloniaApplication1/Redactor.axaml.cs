using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using AvaloniaApplication1.Models;
using AvaloniaApplication1;
using AvaloniaApplication1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using static AvaloniaApplication1.SavingDate;
using Bitmap = Avalonia.Media.Imaging.Bitmap;

namespace AvaloniaApplication1;

public partial class Redactor : Window
{

    private string? _PictureFile = null;//_RedClient != null ? _RedClient.Photo : null; //изображение, которое изначально имеет объект (если оно у него есть, иначе null)
    private string? _SelectedImage = null; //выбранное изображение
    public List<ListTag> _AllClientss = Helper.user.ListTags.Where(x => x.IdClient == _RedClient.Id).ToList(); //Список клиентов из БД (также связанными данными заполнены коллекции в объектах: теги, посещения, файлы)    
                                                                                                               // public List<ListTag> _AllClientss = Helper.user.ListTags.ToList();
    private readonly FileDialogFilter fileFilter = new() //Фильтр для проводника
    {
        Extensions = new List<string>() { "jpg", "png" }, //доступные расширения, отображаемые в проводнике
        Name = "Файлы изображений" //пояснение
    };
    public Redactor()
    {

        InitializeComponent();
        List<ListTag> dff = SavingDate.listtag.Where(x => x.IdClient == SavingDate._RedClient.Id).ToList();
        // List<Tag> tagpers = new List<Tag>();
        //for (int i = 0; i < dff.Count -1; i++)
        //{
        //  tagpers.AddRange(tagpers);
        //tagpers[i].Id =(int)dff[i].IdTag;
        //}

        //Listins(tagpers);
        SelectedClientDataInsertion();
        Listins(_AllClientss);
    }
    private bool NamesCheck(string name)// проверка на корректность ФИО
    {
        return new string[]
        { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
                "`" , "~", "!", "@", "\"", "#", "№", "$", ";", "%",
                "^", ":" , "&", "?", "*", "(", ")", "_", "+", "=",
                "'", "|", "/", "<", ">"
        }.Any(name.Contains);
    }
    private bool NumberCheck(string name)// проверка на корректность ФИО
    {
        return new string[]
        { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P",
               "A", "S", "D", "F", "G", "H", "J", "K", "L", "Z",
                "X", "C", "V", "B", "N", "M", "Й", "Ц", "У", "К",
                 "Е", "Н", "Г", "Ш", "Щ", "З", "Х", "Ъ", "Ф", "Ы",
                  "В", "А", "П", "Р", "О", "Л", "Д", "Ж", "Э", "Я",
                   "Ч", "С", "М", "И", "Т", "Ь", "Б", "Ю",
                "`" , "~", "!", "@", "\"", "#", "№", "$", ";", "%",
                "^", ":" , "&", "?", "*", "_", "=",
                "'", "|", "/", "<", ">"
        }.Any(name.Contains);
    }
    private bool EmailCheck(string number)// проверка на корректность ФИО
    {
        return new string[]
        { "@","."
        }.Any(number.Contains);
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (SavingDate._RedClient == null)
        {
            if (Name.Text != "" && Familia.Text != "" && Otchestvo.Text != "" && Email.Text != "" && Telefon.Text != "" && NamesCheck(Name.Text) == false && EmailCheck(Email.Text) == true && NumberCheck(Telefon.Text) == false && NamesCheck(Otchestvo.Text) == false && NamesCheck(Familia.Text) == false)
            {
                Клиенты newClient = new Клиенты();
                newClient.Id = SavingDate.klient.Count() + 1;
                //newClient.RegistrationDate = _RedClient == null ? new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day) : _RedClient.RegistrationDate;

                newClient.MiddleName = Otchestvo.Text;
                newClient.Name = Name.Text;
                newClient.Surname = Familia.Text;
                newClient.EmailAdress = Email.Text;
                newClient.NumberPhone = Telefon.Text;
                newClient.Gender = tgswictch_gender.IsChecked == true ? 1 : 2;
                newClient.Birthday = calendar_birthday.SelectedDate != null ?
                   new DateOnly(calendar_birthday.SelectedDate.Value.Year, calendar_birthday.SelectedDate.Value.Month, calendar_birthday.SelectedDate.Value.Day) : //Если не была выбрана иная дата, устанавливается сегодняшняя
                 new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                newClient.Photo = _SelectedImage;
                Helper.user.Клиентыs.Add(newClient); //добавление в БД
                Helper.user.SaveChanges(); //сохранение изменений
            }

        }
        else
        {

            //newClient.RegistrationDate = _RedClient == null ? new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day) : _RedClient.RegistrationDate;

            SavingDate._RedClient.MiddleName = Otchestvo.Text;
            SavingDate._RedClient.Name = Name.Text;
            SavingDate._RedClient.Surname = Familia.Text;
            SavingDate._RedClient.EmailAdress = Email.Text;
            SavingDate._RedClient.NumberPhone = Telefon.Text;
            SavingDate._RedClient.Gender = tgswictch_gender.IsChecked == true ? 1 : 2;
            SavingDate._RedClient.Birthday = calendar_birthday.SelectedDate != null ?
               new DateOnly(calendar_birthday.SelectedDate.Value.Year, calendar_birthday.SelectedDate.Value.Month, calendar_birthday.SelectedDate.Value.Day) : //Если не была выбрана иная дата, устанавливается сегодняшняя
             new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            SavingDate._RedClient.Photo = _SelectedImage;
            //Helper.user.Клиентыs.Add(newClient); //добавление в БД
            Helper.user.SaveChanges(); //сохранение изменений
        }

    }

    [Obsolete]
    private async void ImageSelection(object? sender, Avalonia.Interactivity.RoutedEventArgs e) //Выбор и удаление изображения
    {
        var btn = (sender as Button)!;
        switch (btn.Name)
        {
            case "btn_addImage": //добавление
                OpenFileDialog dialog = new(); //Открытие проводника
                dialog.Filters.Add(fileFilter); //Применение фильтра
                string[] result = await dialog.ShowAsync(this); //Выбор файла
                if (result == null || result.Length == 0 || new System.IO.FileInfo(result[0]).Length > 2000000)
                    return;//Если закрыть проводник или размер файла будет превышать 2 МБ, то картинка не будет выбрана

                string imageName = System.IO.Path.GetFileName(result[0]); //получение имени файла
                string[] extention = imageName.Split('.'); //Название файла делится на название и расширение
                string temp = extention[0]; //В изменяемой переменной хранится название файла. Оно будет меняться в процессе
                int i = 1; //Счетчик
                           // while (SameName(temp) != null) //Пока метод для проверки уникальности файла возвращает название файла
                           //{
                           //      temp = extention[0] + $"{i}"; //Новое имя файла
                           //  i++;
                           // }
                imageName = temp + '.' + extention[1]; //Новое имя файла с расширением

                System.IO.File.Copy(result[0], $"Assets/{imageName}", true); //Копирование файла в папку ассетов

                tblock_clientPhoto.Text = imageName;
                image_clientPhoto.Source = new Bitmap($"Assets/{imageName}");
                tblock_clientPhoto.IsVisible = image_clientPhoto.IsVisible = btn_deleteImage.IsVisible = true;
                if (_SelectedImage != null && _SelectedImage != _PictureFile) //Если до установки новой картинки была выбрана другая, и при этом выбранная картинка не значение из поля, хранящее изначальноне изображение товара
                    System.IO.File.Delete($"Assets/{_SelectedImage}"); //Удаление предыдущего изображения из ассетов
                _SelectedImage = imageName;

                break;
            case "btn_deleteImage": //удаление
                tblock_clientPhoto.IsVisible = image_clientPhoto.IsVisible = btn_deleteImage.IsVisible = false;

                // if (_SelectedImage != _PictureFile) //Удаление произойдет только если удаляемое изображение не является значением из поля, хранящее изначальноне изображение объекта
                System.IO.File.Delete($"Assets/{_SelectedImage}"); //Удаление выбранного изображения
                                                                   //сохранение изменений
                                                                   //  _//SelectedImage = null;//очистка поля с выбранным изображением
                break;
        }
    }

    private void SelectedClientDataInsertion()
    {
        //Listins(SavingDate.listtag);
        if (SavingDate._RedClient != null) //если объект оедактируется
        {
            Id.Text = $"ID: {SavingDate._RedClient.Id}"; //Отображается ID
            Otchestvo.Text = SavingDate._RedClient.MiddleName; //поля заполняются соответсвующими данными
            Name.Text = SavingDate._RedClient.Name;
            Familia.Text = SavingDate._RedClient.Surname;

            Telefon.Text = SavingDate._RedClient.NumberPhone;
            Email.Text = SavingDate._RedClient.EmailAdress;

            tgswictch_gender.IsChecked = SavingDate._RedClient.Gender == 1 ? true : false; //отображение пола
            calendar_birthday.DisplayDate = SavingDate._RedClient.Birthday.ToDateTime(TimeOnly.MinValue); //На календре открывается страница с датой рождения и выбирается она же
            calendar_birthday.SelectedDate = SavingDate._RedClient.Birthday.ToDateTime(TimeOnly.MinValue);


            image_clientPhoto.Source = new Bitmap($"Assets/{SavingDate._RedClient.Photo}"); //устаанавливается на превью
            tblock_clientPhoto.Text = _SelectedImage = SavingDate._RedClient.Photo; //передаются значения в текстблок-превью и поле для выбранного изображения
            tblock_clientPhoto.IsVisible = image_clientPhoto.IsVisible = btn_deleteImage.IsVisible = true; //превью и кнопка удаления картинки становится видимыми

        }
    }
    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }
    private void Listins(List<ListTag> list)
    {


        TagList.ItemsSource = list.Select(x => new
        {
            tag[(int)x.IdTag - 1].Name,
            Color = $"{tag[(int)x.IdTag - 1].Color}",
            //tag[(int)y.IdTag - 1].Color,   
        });
    }

    private void Button_Click_2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Helper.user.Клиентыs.Remove(Helper.user.Клиентыs.Find(SavingDate._RedClient.Id)); //В БД находит объект и удаляет
        Helper.user.SaveChanges(); //сохранение изменений
        new MainWindow().Show();
        Close();//Переход к основному окну
    }

    private void Button_Click_3(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new RedactTag().Show();
        Close();
    }
}