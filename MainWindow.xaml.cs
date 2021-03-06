﻿using Microsoft.Win32;
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

namespace EncryptionTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            rdFile.Checked += rdFile_Checked;
            rdText.Checked += rdFile_Checked;
            btnAsymUp.Click += btnAsymUp_Click;
            btnSymUp.Click += btnSymUp_Click;
            this.Loaded += MainWindow_Loaded;
        }

        private void HandleExeption(Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
        }

        void btnSymUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Multiselect = false;
                bool? rslt = dlg.ShowDialog();
                if ((rslt.HasValue) && (rslt.Value))
                {
                    txtSymFile.Text = dlg.FileName;
                }
            }
            catch (Exception ex)
            {
                HandleExeption(ex);
            }
        }

        void btnAsymUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Multiselect = false;
                bool? rslt = dlg.ShowDialog();
                if ((rslt.HasValue) && (rslt.Value))
                {
                    txtAsymFile.Text = dlg.FileName;
                }
            }
            catch (Exception ex)
            {
                HandleExeption(ex);
            }
        }

        void rdFile_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                symOutput.Text = string.Empty;
                AsymInput.Text = string.Empty;
                txtAsymFile.Text = string.Empty;
                txtSymFile.Text = string.Empty;
                if (rdText.IsChecked.Value)
                {
                    symInput.Visibility = System.Windows.Visibility.Visible;
                    AsymInput.Visibility = System.Windows.Visibility.Visible;
                    stckAsym.Visibility = System.Windows.Visibility.Hidden;
                    stckSym.Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    symInput.Visibility = System.Windows.Visibility.Hidden;
                    AsymInput.Visibility = System.Windows.Visibility.Hidden;
                    stckAsym.Visibility = System.Windows.Visibility.Visible;
                    stckSym.Visibility = System.Windows.Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                HandleExeption(ex);
            }
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateComboBoxes();
        }

        void PopulateComboBoxes()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                cmbASym.DataContext = BusObj.Asym.AsymBase.GetASymmetricTypes();
                cmbSym.DataContext = BusObj.Sym.SymBase.GetSymmetricTypes();
                cmbASym.SelectedIndex = 0;
                cmbSym.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                HandleExeption(ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void btnConvertAsym_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BusObj.Asym.AsymBase hshTyp = BusObj.Asym.AsymBase.GetInstance(cmbASym.SelectedValue.ToString());
                if (rdText.IsChecked.Value)
                {
                    if (!string.IsNullOrEmpty(AsymInput.Text))
                    {
                        asymOutput.Text = hshTyp.ComputeHash(AsymInput.Text);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(txtAsymFile.Text))
                    {
                        string inputFile = txtAsymFile.Text;
                        string outputFile = System.IO.Path.ChangeExtension(inputFile, "chng");
                        if (System.IO.File.Exists(inputFile))
                        {
                            byte[] b = hshTyp.ConvertFileToBytes(inputFile);
                            byte[] encryptedBytes = hshTyp.ComputeHash(b);
                            hshTyp.ConvertBytesToFile(encryptedBytes, outputFile);
                            asymOutput.Text = outputFile;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleExeption(ex);
            }
        }

        private void btnConvertSym_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BusObj.Sym.SymBase symTyp = BusObj.Sym.SymBase.GetInstance(cmbSym.SelectedValue.ToString());
                if (rdText.IsChecked.Value)
                {
                    if ((!string.IsNullOrEmpty(symInput.Text)) && (!string.IsNullOrEmpty(txtPass.Text)))
                    {
                        symOutput.Text = symTyp.Encrypt(symInput.Text, txtPass.Text);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(txtSymFile.Text))
                    {
                        string inputFile = txtSymFile.Text;
                        string outputFile = System.IO.Path.ChangeExtension(inputFile, "chng");
                        if (System.IO.File.Exists(inputFile))
                        {
                            byte[] b = symTyp.ConvertFileToBytes(inputFile);
                            byte[] encryptedBytes = symTyp.Encrypt(b, txtPass.Text);
                            symTyp.ConvertBytesToFile(encryptedBytes, outputFile);
                            symOutput.Text = outputFile;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleExeption(ex);
            }
        }
    }
}
