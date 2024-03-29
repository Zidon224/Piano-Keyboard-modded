﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PianoKeyboard
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public Extensions.NoteConverter noteConverter = new Extensions.NoteConverter();
        public Extensions.WinMMManager winMM;
        public Extensions.KeyboardRender keyboardRender;
        public Extensions.KeyLayout keyLayout;

        private SettingWindow setting;
        private ReadmeWindow readme = new ReadmeWindow();
        private AboutBox about;

        public MainWindow()
        {
            InitializeComponent();

            winMM = new Extensions.WinMMManager(this, false, false);
            keyboardRender = new Extensions.KeyboardRender(this);
            keyLayout = new Extensions.KeyLayout(this);

            setting = new SettingWindow(this);
            about = new AboutBox();

            KeyDown += keyLayout.KeyDownHandler;
            KeyUp += keyLayout.KeyUpHandler;

            Keyboard.MouseLeave += keyboardRender.MouseLeaveHandler;
            Keyboard.MouseDown += keyboardRender.MouseDownHandler;
            Keyboard.MouseMove += keyboardRender.MouseMoveHandler;
            Keyboard.MouseUp += keyboardRender.MouseUpHandler;

            Closing += (s, e) =>
            {
                Application.Current.Shutdown();
            };
        }

        public void NoteOnHandler(byte cc, Extensions.NoteConverter.NoteMap note, byte vel = 0x7F, Extensions.KeyboardRender.ColorMap color = new Extensions.KeyboardRender.ColorMap())
        {
            winMM.NoteOn(cc, note.note, vel);
            keyboardRender.ChangeKeyColor(note, true, color);
        }

        public void NoteOffHandler(byte cc, Extensions.NoteConverter.NoteMap note, byte vel = 0x00)
        {
            winMM.NoteOff(cc, note.note);
            keyboardRender.ChangeKeyColor(note, false);
        }
        private void OpenItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Load File";
            openFileDialog.Filter = "MIDI files|*.mid";
            openFileDialog.InitialDirectory = "Desktop";
            openFileDialog.ShowDialog();
        }

        private void SettingItem_Click(object sender, RoutedEventArgs e)
        {
            setting.ShowDialog();
        }

        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void AboutItem_Click(object sender, RoutedEventArgs e)
        {
            about.ShowDialog();
        }

        private void ReadmeItem_Click(object sender, RoutedEventArgs e)
        {
            readme.Show();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
