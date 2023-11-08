using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using ScaleformUI.Menu;
using ScaleformUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vMenu.Client.Functions;
using vMenu.Client.Settings;

namespace vMenu.Client.Menus
{
    public class RecordingMenu : BaseScript
    {
        public static MenuFunctions MenuFunctions = new MenuFunctions();

        private static UIMenu recordingMenu = null;

        public RecordingMenu()
        {
            AddTextEntryByHash(0x86F10CE6, "Upload To Cfx.re Forum"); // Replace the "Upload To Social Club" button in gallery
            AddTextEntry("ERROR_UPLOAD", "Are you sure you want to upload this photo to Cfx.re forum?"); // Replace the warning message text for uploading

            recordingMenu = new UIMenu(Main.MenuBanner.BannerTitle, "Recording Options", MenuFunctions.GetMenuOffset(), Main.MenuBanner.TextureDictionary, Main.MenuBanner.TextureName, false, true, fadingTime: 0.01f)
            {
                MaxItemsOnScreen = MenuSettings.MaxItemsOnScreen,
                BuildingAnimation = MenuSettings.BuildingAnimation,
                ScrollingType = MenuSettings.ScrollingType,
                Enabled3DAnimations = MenuSettings.Enabled3DAnimations,
                MouseControlsEnabled = MenuSettings.MouseControlsEnabled,
                MouseEdgeEnabled = MenuSettings.MouseEdgeEnabled,
                MouseWheelControlEnabled = MenuSettings.MouseWheelControlEnabled,
                ControlDisablingEnabled = MenuSettings.ControlDisablingEnabled,
                EnableAnimation = MenuSettings.EnableAnimation,
            };

            UIMenuItem takePicture = new UIMenuItem("Take Photo", "Takes a photo and saves it to the Pause Menu gallery.", MenuSettings.BackgroundColor, MenuSettings.HighlightColor);
            UIMenuItem openPmGallery = new UIMenuItem("Open Gallery", "Opens the Pause Menu gallery.", MenuSettings.BackgroundColor, MenuSettings.HighlightColor);
            UIMenuItem startRecording = new UIMenuItem("Start Recording", "Start a new game recording using GTA V's built in recording.", MenuSettings.BackgroundColor, MenuSettings.HighlightColor);
            UIMenuItem stopRecording = new UIMenuItem("Stop Recording", "Stop and save your current recording.", MenuSettings.BackgroundColor, MenuSettings.HighlightColor);
            UIMenuItem openRockstarEditor = new UIMenuItem("Rockstar Editor", "Open the Rockstar Editor, note you might want to quit the session first before doing this to prevent some issues.", MenuSettings.BackgroundColor, MenuSettings.HighlightColor);
            recordingMenu.AddItem(takePicture);
            recordingMenu.AddItem(openPmGallery);
            recordingMenu.AddItem(startRecording);
            recordingMenu.AddItem(stopRecording);
            recordingMenu.AddItem(openRockstarEditor);

            takePicture.Activated += (sender, i) =>
            {
                BeginTakeHighQualityPhoto();
                SaveHighQualityPhoto(-1);
                FreeMemoryForHighQualityPhoto();                
            };

            openPmGallery.Activated += (sender, i) =>
            {
                ActivateFrontendMenu((uint)GetHashKey("FE_MENU_VERSION_MP_PAUSE"), true, 3);           
            };

            startRecording.Activated += (sender, i) =>
            {
                if (IsRecording())
                {
                    Notify.Error("You are already recording a clip, you need to stop recording first before you can start recording again!", true, false);  
                }
                else
                {
                    StartRecording(1);
                }                
            };

            stopRecording.Activated += (sender, i) =>
            {
                if (!IsRecording())
                {
                    Notify.Error("You are currently NOT recording a clip, you need to start recording first before you can stop and save a clip.", true, false);  
                }
                else
                {
                    StopRecordingAndSaveClip();
                }                
            };

            openRockstarEditor.Activated += async (sender, i) =>
            {
                if (true)
                {
                    //MenuFunctions.QuitSession();
                }
                ActivateRockstarEditor();
                // wait for the editor to be closed again.
                while (IsPauseMenuActive())
                {
                    await BaseScript.Delay(0);
                }
                // then fade in the screen.
                DoScreenFadeIn(1);
                Notify.Alert("You left your previous session before entering the Rockstar Editor. Restart the game to be able to rejoin the server's main session.", true, true);         
            };

            Main.Menus.Add(recordingMenu);
        }

        public static UIMenu Menu()
        {
            return recordingMenu;
        }
    }
}