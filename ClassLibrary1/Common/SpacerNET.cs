﻿
using SpacerUnion.Common;
using SpacerUnion.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpacerUnion
{

    public static class SpacerNET
    {

        // Окна приложения
        public static MainForm form;
        public static VobListForm vobList;
        public static ObjectsWindow propWin;
        public static InfoWin infoWin;
        public static CompileLightWin comLightWin;
        public static ObjTree objTreeWin;
        public static ObjectsWin objectsWin;
        public static SoundWin soundWin;
        public static CompileWorldWin compWorldWin;
        public static LoadingForm loadForm;
        public static SettingsCamera settingsCam;
        public static SettingsControls settingsControl;
        public static CameraCoords camCoordsWin;
        public static MiscSettingsWin miscSetWin;
        public static KeysForm keysWin;
        public static PFXEditorWin pfxWin;
        public static GrassWin grassWin;

        // Список скрытых окон
        static List<Form> windowsToHideList = null;

        // Список всех окон
        static List<Form> windowsList = null;

        public static bool isInit = false;


        [DllExport]
        public static void UI_Initialize()
        {
            Imports.CoInitialize(IntPtr.Zero);
            Application.EnableVisualStyles();
        }

        // Главная функция запуска, вызывается из Union
        [DllExport]
        public static void UnionInitialize()
        {
         
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Localizator.Init();
            Localizator.SetLanguage((LangEnum)Properties.Settings.Default.Language);
           
            
            
            windowsToHideList = new List<Form>();
            windowsList = new List<Form>();

            form = new MainForm();
            vobList = new VobListForm();
            propWin = new ObjectsWindow();
            infoWin = new InfoWin();
            objTreeWin = new ObjTree();
            objectsWin = new ObjectsWin();
            soundWin = new SoundWin();
            comLightWin = new CompileLightWin();
            settingsCam = new SettingsCamera();
            settingsControl = new SettingsControls();
            compWorldWin = new CompileWorldWin();
            loadForm = new LoadingForm();
            camCoordsWin = new CameraCoords();
            miscSetWin = new MiscSettingsWin();
            keysWin = new KeysForm();
            pfxWin = new PFXEditorWin();
            grassWin = new GrassWin();

            windowsList.Add(objTreeWin);
            windowsList.Add(objectsWin);
            windowsList.Add(infoWin);
            windowsList.Add(propWin);
            windowsList.Add(soundWin);
            windowsList.Add(vobList);
            windowsList.Add(compWorldWin);
            windowsList.Add(comLightWin);
            windowsList.Add(settingsCam);
            windowsList.Add(loadForm);
            windowsList.Add(settingsControl);
            windowsList.Add(camCoordsWin);
            windowsList.Add(miscSetWin);
            windowsList.Add(keysWin);
            windowsList.Add(pfxWin);
            windowsList.Add(grassWin);

            

            // каждому окну из списка задаем владельца: главную форму
            windowsList.ForEach(x => x.Owner = form);

            form.menuStripTopMain.Enabled = false;

            Localizator.UpdateInterface();

            form.Show();

            Size resolution = Screen.PrimaryScreen.Bounds.Size;

            if (Properties.Settings.Default.MainWindowPos != null)
            {
                form.Location = Properties.Settings.Default.MainWindowPos;

                if (form.Location.X < -resolution.Width)
                {
                    form.Location = new Point(0, 0);
                }

                if (form.Location.Y < -resolution.Height)
                {
                    form.Location = new Point(0, 0);
                }
            }
            else
            {
                form.Location = new Point(0, 0);
            }


            if (Properties.Settings.Default.MainWindowSize != null)
            {
                form.Size = Properties.Settings.Default.MainWindowSize;
            }
            else
            {
                
                form.Size = resolution;
            }

            

            if (Properties.Settings.Default.MainWindowMaxState)
            {
                form.WindowState = FormWindowState.Maximized;
            }
            else
            {
                form.WindowState = FormWindowState.Normal;
            }
           

            form.AddText(Localizator.Get("appIsLoading"));


        }

        // Функция вызывается, когда загрузился движок игры (Game_Init), вызывается из Union
        [DllExport]
        public static void Form_EnableInterface()
        {
            //ConsoleEx.WriteLineRed("Form_EnableInterface");
            //Utils.Error("Form_EnableInterface");
            form.AddText(Localizator.Get("appIsReady"), Color.Green);

            form.menuStripTopMain.Enabled = true;


            if (Properties.Settings.Default.InfoWinShow || Properties.Settings.Default.InfoWinShowNext)
            {
                infoWin.Show();
            }

            if (Properties.Settings.Default.ObjectWinShow || Properties.Settings.Default.ObjWinShowNext)
            {
                objectsWin.Show();
            }

            if (Properties.Settings.Default.PropsWinShow || Properties.Settings.Default.PropWinShowNext)
            {
                propWin.Show();
            }

            if (Properties.Settings.Default.SoundWinShow || Properties.Settings.Default.SoundWinShowNext)
            {
                soundWin.Show();
            }

            if (Properties.Settings.Default.TreeWinShow || Properties.Settings.Default.TreeWinShowNext)
            {
                objTreeWin.Show();
            }

            if (Properties.Settings.Default.VobListWinShow || Properties.Settings.Default.VobListWinShowNext)
            {
                vobList.Show();
            }


            if (Properties.Settings.Default.GrassWinShow || Properties.Settings.Default.GrassWinShowNext)
            {
                grassWin.Show();
            }



            if (Properties.Settings.Default.TreeWinLocation != null)
            {
                objTreeWin.Location = Properties.Settings.Default.TreeWinLocation;
            }

            if (Properties.Settings.Default.PartWinLocation != null)
            {
                objectsWin.Location = Properties.Settings.Default.PartWinLocation;
            }

            if (Properties.Settings.Default.VobListWinLocation != null)
            {
                vobList.Location = Properties.Settings.Default.VobListWinLocation;
            }

            if (Properties.Settings.Default.PropWinLocation != null)
            {
                propWin.Location = Properties.Settings.Default.PropWinLocation;
            }


            if (Properties.Settings.Default.InfoWinLocation != null)
            {
                infoWin.Location = Properties.Settings.Default.InfoWinLocation;
            }


            if (Properties.Settings.Default.SoundWinLocation != null)
            {
                soundWin.Location = Properties.Settings.Default.SoundWinLocation;
            }

            if (Properties.Settings.Default.GrassWinLocation != null)
            {
                grassWin.Location = Properties.Settings.Default.GrassWinLocation;
            }

            if (Properties.Settings.Default.TreeWinSize != null)
            {
                objTreeWin.Size = Properties.Settings.Default.TreeWinSize;
            }

            if (Properties.Settings.Default.PropsWinSize != null)
            {
                propWin.Size = Properties.Settings.Default.PropsWinSize;
            }

            if (Properties.Settings.Default.VobListSize != null)
            {
                vobList.Size = Properties.Settings.Default.VobListSize;
            }



            LoadSettingsToInterface();




            form.Focus();
        }

        


        // загружает настройки в интерфейс 
        public static void LoadSettingsToInterface()
        {
            //ConsoleEx.WriteLineRed("LoadSettingsToInterface");
            ToolStripButton btn = null;


            Imports.Stack_PushString("grassMinDist");
            Imports.Stack_PushString("vobListRadius");
            Imports.Stack_PushString("bToggleWorkMode");
            Imports.Stack_PushString("showInvisibleVobs");
            Imports.Stack_PushString("drawBBoxGlobal");
            Imports.Stack_PushString("showHelpVobs");
            Imports.Stack_PushString("showWaynet");
            Imports.Stack_PushString("showVobs");
            

            btn = form.toolStripTop.Items[7] as ToolStripButton;
            btn.Checked = Convert.ToBoolean(Imports.Extern_GetSetting());

            btn = form.toolStripTop.Items[8] as ToolStripButton;
            btn.Checked = Convert.ToBoolean(Imports.Extern_GetSetting());

            btn = form.toolStripTop.Items[9] as ToolStripButton;
            btn.Checked = Convert.ToBoolean(Imports.Extern_GetSetting());

            btn = form.toolStripTop.Items[10] as ToolStripButton;
            btn.Checked = Convert.ToBoolean(Imports.Extern_GetSetting());

            btn = form.toolStripTop.Items[11] as ToolStripButton;
            btn.Checked = Convert.ToBoolean(Imports.Extern_GetSetting());



            int workMode = Imports.Extern_GetSetting(); //bToggleWorkMode

            if (workMode == 0)
            {

            }
            else if (workMode == 1)
            {
                btn = form.toolStripTop.Items[14] as ToolStripButton; //mat
                btn.Checked = true;
            }
            else if (workMode == 2)
            {
                btn = form.toolStripTop.Items[15] as ToolStripButton; //grass
                btn.Checked = true;
            }


            int radius = Imports.Extern_GetSetting();
            vobList.trackBarRadius.Value = radius;

            int radiusGrassMin = Imports.Extern_GetSetting();
            grassWin.trackBarWinGrassMinRadius.Value = radius;


            Imports.Stack_PushString("grassModelName");
            Imports.Extern_GetSettingStr();

            grassWin.textBoxGrassWinModel.Text = Imports.Stack_PeekString();

            //==========================================================


            btn = form.toolStripTop.Items[0] as ToolStripButton;
            btn.Checked = Convert.ToBoolean(infoWin.Visible);

            btn = form.toolStripTop.Items[1] as ToolStripButton;
            btn.Checked = Convert.ToBoolean(objectsWin.Visible);

            btn = form.toolStripTop.Items[2] as ToolStripButton;
            btn.Checked = Convert.ToBoolean(soundWin.Visible);

            btn = form.toolStripTop.Items[3] as ToolStripButton;
            btn.Checked = Convert.ToBoolean(objTreeWin.Visible);

            btn = form.toolStripTop.Items[4] as ToolStripButton;
            btn.Checked = Convert.ToBoolean(propWin.Visible);


            btn = form.toolStripTop.Items[5] as ToolStripButton;
            btn.Checked = Convert.ToBoolean(vobList.Visible);



            var some = form.freezeTimeToolStripMenuItem;

            Imports.Stack_PushString("holdTime");

            int holdTime = Imports.Extern_GetSetting();
            form.freezeTimeToolStripMenuItem.Checked = Convert.ToBoolean(holdTime);


            form.normalToolStripMenuItem.Checked = true;

            form.ResetInterface();
            objectsWin.LoadSettings();
            keysWin.LoadKeys();

            isInit = true;
        }



        public static void CloseApplication()
        {
            Properties.Settings.Default.TreeWinLocation = objTreeWin.Location;
            Properties.Settings.Default.PartWinLocation = objectsWin.Location;
            Properties.Settings.Default.PropWinLocation = propWin.Location;
            Properties.Settings.Default.VobListWinLocation = vobList.Location;
            Properties.Settings.Default.InfoWinLocation = infoWin.Location;
            Properties.Settings.Default.SoundWinLocation = soundWin.Location;


            Properties.Settings.Default.InfoWinShow = infoWin.Visible;
            Properties.Settings.Default.ObjectWinShow = objectsWin.Visible;
            Properties.Settings.Default.PropsWinShow = propWin.Visible;
            Properties.Settings.Default.SoundWinShow = soundWin.Visible;
            Properties.Settings.Default.TreeWinShow = objTreeWin.Visible;
            Properties.Settings.Default.VobListWinShow = vobList.Visible;


            Properties.Settings.Default.ObjWinShowNext = !objectsWin.Visible && windowsToHideList.Contains(objectsWin);
            Properties.Settings.Default.VobListWinShowNext = !vobList.Visible && windowsToHideList.Contains(vobList);
            Properties.Settings.Default.TreeWinShowNext = !objTreeWin.Visible && windowsToHideList.Contains(objTreeWin);
            Properties.Settings.Default.SoundWinShowNext = !soundWin.Visible && windowsToHideList.Contains(soundWin);
            Properties.Settings.Default.PropWinShowNext = !propWin.Visible && windowsToHideList.Contains(propWin);
            Properties.Settings.Default.InfoWinShowNext = !infoWin.Visible && windowsToHideList.Contains(infoWin);


            Properties.Settings.Default.MainWindowPos = form.Location;
            Properties.Settings.Default.MainWindowSize = form.Size;
            Properties.Settings.Default.MainWindowMaxState = (form.WindowState == FormWindowState.Maximized);


            Properties.Settings.Default.TreeWinSize = objTreeWin.Size;
            Properties.Settings.Default.PropsWinSize = propWin.Size;
            Properties.Settings.Default.VobListSize = vobList.Size;


            Properties.Settings.Default.Save();



            Imports.Extern_Exit();
        }


        [DllExport]
        public static int GetHandlerFunc()
        {
            return (int)form.renderTarget.Handle;
        }

        

        [DllExport]
        public static int IsAppActive(IntPtr handle)
        {
            //var handle = Imports.GetForegroundWindow();

            for (int i = 0; i < windowsList.Count; i++)
            {
                if (windowsList[i].Handle == handle || handle == form.Handle || handle == form.renderTarget.Handle)
                {
                    return 1;
                }
            }
            return 0;
        }

        [DllExport]
        public static int GetMainWinHandler()
        {
            return (int)form.Handle;
        }

        [DllExport]
        public static int GetVobsWinHandler()
        {
            return (int)objTreeWin.Handle;
        }


        [DllExport]
        public static void HideWindows()
        {
                
            for (int i = 0; i < windowsList.Count; i++)
            {
                if (windowsList[i].Visible)
                {
                    windowsToHideList.Add(windowsList[i]);
                    windowsList[i].Hide();
                }
                
            }

        }

        [DllExport]
        public static void ShowWindows()
        {

            for (int i = 0; i < windowsToHideList.Count; i++)
            {
                windowsToHideList[i].Show();
            }

            windowsToHideList.Clear();
            form.Focus();
        }



        [DllExport]
        public static void ShowLoadingForm(int type)
        {
            if (loadForm == null)
            {
                loadForm = new LoadingForm();
            }

            loadForm.UpdateText(type);
            loadForm.buttonLoadingFormClose.Visible = false;
            loadForm.Show();
            Application.DoEvents();
        }




        [DllExport]
        public static void CloseLoadingForm()
        {
            if (loadForm != null)
            {
                loadForm.Hide();
                loadForm.buttonLoadingFormClose.Visible = true;
            }

            Application.DoEvents();

        }


        [DllExport]
        public static void AcceptKey(int key)
        {
            //ConsoleEx.WriteLineRed("Union key: " + key);

        }

    }

}
