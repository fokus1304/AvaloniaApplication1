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

    private string? _PictureFile = null;//_RedClient != null ? _RedClient.Photo : null; //�����������, ������� ���������� ����� ������ (���� ��� � ���� ����, ����� null)
    private string? _SelectedImage = null; //��������� �����������
    public List<ListTag> _AllClientss = Helper.user.ListTags.Where(x => x.IdClient == _RedClient.Id).ToList(); //������ �������� �� �� (����� ���������� ������� ��������� ��������� � ��������: ����, ���������, �����)    
                                                                                                               // public List<ListTag> _AllClientss = Helper.user.ListTags.ToList();
    private readonly FileDialogFilter fileFilter = new() //������ ��� ����������
    {
        Extensions = new List<string>() { "jpg", "png" }, //��������� ����������, ������������ � ����������
        Name = "����� �����������" //���������
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
    private bool NamesCheck(string name)// �������� �� ������������ ���
    {
        return new string[]
        { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
                "`" , "~", "!", "@", "\"", "#", "�", "$", ";", "%",
                "^", ":" , "&", "?", "*", "(", ")", "_", "+", "=",
                "'", "|", "/", "<", ">"
        }.Any(name.Contains);
    }
    private bool NumberCheck(string name)// �������� �� ������������ ���
    {
        return new string[]
        { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P",
               "A", "S", "D", "F", "G", "H", "J", "K", "L", "Z",
                "X", "C", "V", "B", "N", "M", "�", "�", "�", "�",
                 "�", "�", "�", "�", "�", "�", "�", "�", "�", "�",
                  "�", "�", "�", "�", "�", "�", "�", "�", "�", "�",
                   "�", "�", "�", "�", "�", "�", "�", "�",
                "`" , "~", "!", "@", "\"", "#", "�", "$", ";", "%",
                "^", ":" , "&", "?", "*", "_", "=",
                "'", "|", "/", "<", ">"
        }.Any(name.Contains);
    }
    private bool EmailCheck(string number)// �������� �� ������������ ���
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
                ������� newClient = new �������();
                newClient.Id = SavingDate.klient.Count() + 1;
                //newClient.RegistrationDate = _RedClient == null ? new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day) : _RedClient.RegistrationDate;

                newClient.MiddleName = Otchestvo.Text;
                newClient.Name = Name.Text;
                newClient.Surname = Familia.Text;
                newClient.EmailAdress = Email.Text;
                newClient.NumberPhone = Telefon.Text;
                newClient.Gender = tgswictch_gender.IsChecked == true ? 1 : 2;
                newClient.Birthday = calendar_birthday.SelectedDate != null ?
                   new DateOnly(calendar_birthday.SelectedDate.Value.Year, calendar_birthday.SelectedDate.Value.Month, calendar_birthday.SelectedDate.Value.Day) : //���� �� ���� ������� ���� ����, ��������������� �����������
                 new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                newClient.Photo = _SelectedImage;
                Helper.user.�������s.Add(newClient); //���������� � ��
                Helper.user.SaveChanges(); //���������� ���������
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
               new DateOnly(calendar_birthday.SelectedDate.Value.Year, calendar_birthday.SelectedDate.Value.Month, calendar_birthday.SelectedDate.Value.Day) : //���� �� ���� ������� ���� ����, ��������������� �����������
             new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            SavingDate._RedClient.Photo = _SelectedImage;
            //Helper.user.�������s.Add(newClient); //���������� � ��
            Helper.user.SaveChanges(); //���������� ���������
        }

    }

    [Obsolete]
    private async void ImageSelection(object? sender, Avalonia.Interactivity.RoutedEventArgs e) //����� � �������� �����������
    {
        var btn = (sender as Button)!;
        switch (btn.Name)
        {
            case "btn_addImage": //����������
                OpenFileDialog dialog = new(); //�������� ����������
                dialog.Filters.Add(fileFilter); //���������� �������
                string[] result = await dialog.ShowAsync(this); //����� �����
                if (result == null || result.Length == 0 || new System.IO.FileInfo(result[0]).Length > 2000000)
                    return;//���� ������� ��������� ��� ������ ����� ����� ��������� 2 ��, �� �������� �� ����� �������

                string imageName = System.IO.Path.GetFileName(result[0]); //��������� ����� �����
                string[] extention = imageName.Split('.'); //�������� ����� ������� �� �������� � ����������
                string temp = extention[0]; //� ���������� ���������� �������� �������� �����. ��� ����� �������� � ��������
                int i = 1; //�������
                           // while (SameName(temp) != null) //���� ����� ��� �������� ������������ ����� ���������� �������� �����
                           //{
                           //      temp = extention[0] + $"{i}"; //����� ��� �����
                           //  i++;
                           // }
                imageName = temp + '.' + extention[1]; //����� ��� ����� � �����������

                System.IO.File.Copy(result[0], $"Assets/{imageName}", true); //����������� ����� � ����� �������

                tblock_clientPhoto.Text = imageName;
                image_clientPhoto.Source = new Bitmap($"Assets/{imageName}");
                tblock_clientPhoto.IsVisible = image_clientPhoto.IsVisible = btn_deleteImage.IsVisible = true;
                if (_SelectedImage != null && _SelectedImage != _PictureFile) //���� �� ��������� ����� �������� ���� ������� ������, � ��� ���� ��������� �������� �� �������� �� ����, �������� ������������ ����������� ������
                    System.IO.File.Delete($"Assets/{_SelectedImage}"); //�������� ����������� ����������� �� �������
                _SelectedImage = imageName;

                break;
            case "btn_deleteImage": //��������
                tblock_clientPhoto.IsVisible = image_clientPhoto.IsVisible = btn_deleteImage.IsVisible = false;

                // if (_SelectedImage != _PictureFile) //�������� ���������� ������ ���� ��������� ����������� �� �������� ��������� �� ����, �������� ������������ ����������� �������
                System.IO.File.Delete($"Assets/{_SelectedImage}"); //�������� ���������� �����������
                                                                   //���������� ���������
                                                                   //  _//SelectedImage = null;//������� ���� � ��������� ������������
                break;
        }
    }

    private void SelectedClientDataInsertion()
    {
        //Listins(SavingDate.listtag);
        if (SavingDate._RedClient != null) //���� ������ �������������
        {
            Id.Text = $"ID: {SavingDate._RedClient.Id}"; //������������ ID
            Otchestvo.Text = SavingDate._RedClient.MiddleName; //���� ����������� ��������������� �������
            Name.Text = SavingDate._RedClient.Name;
            Familia.Text = SavingDate._RedClient.Surname;

            Telefon.Text = SavingDate._RedClient.NumberPhone;
            Email.Text = SavingDate._RedClient.EmailAdress;

            tgswictch_gender.IsChecked = SavingDate._RedClient.Gender == 1 ? true : false; //����������� ����
            calendar_birthday.DisplayDate = SavingDate._RedClient.Birthday.ToDateTime(TimeOnly.MinValue); //�� �������� ����������� �������� � ����� �������� � ���������� ��� ��
            calendar_birthday.SelectedDate = SavingDate._RedClient.Birthday.ToDateTime(TimeOnly.MinValue);


            image_clientPhoto.Source = new Bitmap($"Assets/{SavingDate._RedClient.Photo}"); //���������������� �� ������
            tblock_clientPhoto.Text = _SelectedImage = SavingDate._RedClient.Photo; //���������� �������� � ���������-������ � ���� ��� ���������� �����������
            tblock_clientPhoto.IsVisible = image_clientPhoto.IsVisible = btn_deleteImage.IsVisible = true; //������ � ������ �������� �������� ���������� ��������

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
        Helper.user.�������s.Remove(Helper.user.�������s.Find(SavingDate._RedClient.Id)); //� �� ������� ������ � �������
        Helper.user.SaveChanges(); //���������� ���������
        new MainWindow().Show();
        Close();//������� � ��������� ����
    }

    private void Button_Click_3(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new RedactTag().Show();
        Close();
    }
}