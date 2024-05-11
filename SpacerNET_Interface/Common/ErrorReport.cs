﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SpacerUnion.Common
{
    public enum ErrorReportType
    {
        ERROR_REPORT_TYPE_NONE = 0,
        ERROR_REPORT_TYPE_INFO,
        ERROR_REPORT_TYPE_WARNING,
        ERROR_REPORT_TYPE_CRITICAL
    }
    public enum ErrorReportProblemType
    {
        ERROR_REPORT_PROBLEM_TYPE_NONE = 0,
        ERROR_REPORT_PROBLEM_TYPE_MESH_MAT_NAME,
        ERROR_REPORT_PROBLEM_TYPE_MESH_MAT_TEXTURE_BAD_NAME,
        ERROR_REPORT_PROBLEM_TYPE_MESH_MAT_TEXTURE_NOT_FOUND,
        ERROR_REPORT_PROBLEM_TYPE_TRIGGER_NO_NAME,
        ERROR_REPORT_PROBLEM_TYPE_PFX_CANT_BE_PARENT,
        ERROR_REPORT_PROBLEM_TYPE_ITEM_CANT_BE_PARENT,
        ERROR_REPORT_PROBLEM_TYPE_ITEM_NO_VISUAL,
        ERROR_REPORT_PROBLEM_TYPE_ZCVOB_EMPTY_VISUAL,
        ERROR_REPORT_PROBLEM_TYPE_FOG_ZONES,
        ERROR_REPORT_PROBLEM_TYPE_VOB_ZONES,
        ERROR_REPORT_PROBLEM_TYPE_MUSIC_ZONES,
        ERROR_REPORT_PROBLEM_TYPE_STARTPOINT,
        ERROR_REPORT_PROBLEM_TYPE_EMPTY_NAME,
        ERROR_REPORT_PROBLEM_TYPE_EMPTY_NAME_MOB_FOCUS,
        ERROR_REPORT_PROBLEM_TYPE_MESH_MAT_COLLISION,
        ERROR_REPORT_PROBLEM_TYPE_NAME_SPACE,
        ERROR_REPORT_PROBLEM_TYPE_NOT_UNIQ_NAME,
        ERROR_REPORT_PROBLEM_TYPE_BAD_NAME_SYMBOLS,
        ERROR_REPORT_PROBLEM_TYPE_NAME_IS_VISUAL
    }

    public class ErrorReportEntry
    {
        private ErrorReportType errorType;
        private ErrorReportProblemType problemType;

        public uint objectAddr;
        public string textureName;
        public string materialName;
        public string vobName;
        public int id;

        public ErrorReportEntry()
        {
            ErrorType = ErrorReportType.ERROR_REPORT_TYPE_NONE;
            ProblemType = ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_NONE;
            ObjectAddr = 0;
            textureName = String.Empty;
            materialName = String.Empty;
            id = 0;
        }

        public uint ObjectAddr { get => objectAddr; set => objectAddr = value; }
        public ErrorReportType ErrorType { get => errorType; set => errorType = value; }
        public ErrorReportProblemType ProblemType { get => problemType; set => problemType = value; }


        public string GetReportTypeText()
        {
            string result = String.Empty;

            switch (errorType)
            {
                case ErrorReportType.ERROR_REPORT_TYPE_INFO: result = Localizator.Get("ERROR_REPORT_TYPE_INFO"); break;
                case ErrorReportType.ERROR_REPORT_TYPE_WARNING: result = Localizator.Get("ERROR_REPORT_TYPE_WARNING"); break;
                case ErrorReportType.ERROR_REPORT_TYPE_CRITICAL: result = Localizator.Get("ERROR_REPORT_TYPE_CRITICAL"); break;

                    
            }


            return result;
        }

        public string GetProblemTypeText()
        {
            string result = String.Empty;

            switch (ProblemType)
            {
                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_MESH_MAT_NAME: result = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_MESH_MAT_NAME"); break;
                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_MESH_MAT_TEXTURE_BAD_NAME: result = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_MESH_MAT_TEXTURE_BAD_NAME"); break;
                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_MESH_MAT_TEXTURE_NOT_FOUND: result = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_MESH_MAT_TEXTURE_NOT_FOUND"); break;
                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_TRIGGER_NO_NAME: result = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_TRIGGER_NO_NAME"); break;
                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_PFX_CANT_BE_PARENT: result = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_PFX_CANT_BE_PARENT"); break;
                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_ITEM_CANT_BE_PARENT: result = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_ITEM_CANT_BE_PARENT"); break;
                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_ITEM_NO_VISUAL: result = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_ITEM_NO_VISUAL"); break;
                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_ZCVOB_EMPTY_VISUAL: result = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_ZCVOB_EMPTY_VISUAL"); break;
                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_FOG_ZONES: result = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_FOG_ZONES"); break;
                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_VOB_ZONES: result = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_VOB_ZONES"); break;
                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_MUSIC_ZONES: result = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_MUSIC_ZONES"); break;
                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_STARTPOINT: result = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_STARTPOINT"); break;
                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_EMPTY_NAME: result = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_EMPTY_NAME"); break;
                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_EMPTY_NAME_MOB_FOCUS: result = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_EMPTY_NAME_MOB_FOCUS"); break;
                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_NAME_SPACE: result = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_NAME_SPACE"); break;
                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_NOT_UNIQ_NAME: result = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_NOT_UNIQ_NAME"); break;
                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_BAD_NAME_SYMBOLS: result = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_BAD_NAME_SYMBOLS"); break;
                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_NAME_IS_VISUAL: result = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_NAME_IS_VISUAL"); break;
                    
            }


            return result;
        }

        public string GetDescriptionText()
        {
            string result = String.Empty;


            var stringFormatMatTex = Localizator.Get("ERROR_REPORT_TEXT_MATERIAL") + ": {0}, " +
                            Localizator.Get("ERROR_REPORT_TEXT_TEXTURE") + ": {1}";


            

            switch (ProblemType)
            {
                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_MESH_MAT_NAME:
                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_MESH_MAT_TEXTURE_BAD_NAME:
                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_MESH_MAT_TEXTURE_NOT_FOUND:
                    {
                            result = String.Format(stringFormatMatTex, materialName, textureName);
                    };
                    break;

                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_TRIGGER_NO_NAME:
                    {
                        result = String.Format(Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_TRIGGER_NO_NAME"));
                    };
                    break;
                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_ITEM_NO_VISUAL:
                    {

                        var stringFormatItemNoVisual = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_ITEM_NO_VISUAL") + ": {0}";

                        result = String.Format(stringFormatItemNoVisual, vobName);
                    };
                    break;
                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_ZCVOB_EMPTY_VISUAL:
                    {

                        var stringFormatItemNoVisual = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_ZCVOB_EMPTY_VISUAL") + ": {0}";

                        result = String.Format(stringFormatItemNoVisual, vobName);
                    };
                    break;
                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_PFX_CANT_BE_PARENT:
                    {

                        var stringFormatPFXParent = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_PFX_CANT_BE_PARENT") + ": {0}";

                        result = String.Format(stringFormatPFXParent, vobName);
                    };
                    break;
                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_ITEM_CANT_BE_PARENT:
                    {

                        var stringFormatPFXParent = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_ITEM_CANT_BE_PARENT") + ": {0}";

                        result = String.Format(stringFormatPFXParent, vobName);
                    };
                    break;
                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_FOG_ZONES:
                    {

                        var stringFormatPFXParent = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_FOG_ZONES") + ": {0}";

                        result = String.Format(stringFormatPFXParent, vobName);
                    };
                    break;

                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_VOB_ZONES:
                    {

                        var stringFormatPFXParent = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_VOB_ZONES") + ": {0}";

                        result = String.Format(stringFormatPFXParent, vobName);
                    };
                    break;

                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_MUSIC_ZONES:
                    {

                        var stringFormatPFXParent = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_MUSIC_ZONES") + ": {0}";

                        result = String.Format(stringFormatPFXParent, vobName);
                    };
                    break;

                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_STARTPOINT:
                    {

                        var stringFormat = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_STARTPOINT") + ": {0}";

                        result = String.Format(stringFormat, vobName);
                    };
                    break;

                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_EMPTY_NAME:
                    {

                        var stringFormat = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_EMPTY_NAME") + ": {0}";

                        result = String.Format(stringFormat, vobName);
                    };
                    break;
                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_EMPTY_NAME_MOB_FOCUS:
                    {

                        var stringFormat = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_EMPTY_NAME_MOB_FOCUS") + ": {0}";

                        result = String.Format(stringFormat, vobName);
                    };
                    break;

                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_NAME_SPACE:
                    {

                        var stringFormatSpaceName = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_NAME_SPACE") + ": {0}";

                        result = String.Format(stringFormatSpaceName, vobName);
                    };
                    break;

                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_NOT_UNIQ_NAME:
                    {

                        var stringFormatSpaceName = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_NOT_UNIQ_NAME") + ": {0}";

                        result = String.Format(stringFormatSpaceName, vobName);
                    };
                    break;

                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_BAD_NAME_SYMBOLS:
                    {

                        var stringFormatSpaceName = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_BAD_NAME_SYMBOLS") + ": {0}";

                        result = String.Format(stringFormatSpaceName, vobName);
                    };
                    break;

                case ErrorReportProblemType.ERROR_REPORT_PROBLEM_TYPE_NAME_IS_VISUAL:
                    {

                        var stringFormatSpaceName = Localizator.Get("ERROR_REPORT_PROBLEM_TYPE_NAME_IS_VISUAL") + ": {0}";

                        result = String.Format(stringFormatSpaceName, vobName);
                    };
                    break;

            }

            return result;
        }

        public string GetLinkText()
        {
            if (materialName.Length > 0)
            {
                return Localizator.Get("ERROR_REPORT_COPY_MAT_NAME");
            }
            else
            {
                return Localizator.Get("ERROR_REPORT_DOUBLE_CLICK");
            }
            
        }

        public Color GetTypeBackColor()
        {
            Color col = Color.Black;

            switch (ErrorType)
            {
                case ErrorReportType.ERROR_REPORT_TYPE_INFO: col = Color.FromArgb(255, 134, 182, 255); break;
                case ErrorReportType.ERROR_REPORT_TYPE_WARNING: col = Color.Orange; break;
                case ErrorReportType.ERROR_REPORT_TYPE_CRITICAL: col = Color.Red; break;
            }

            return col;
        }

        public void SetErrorType(ErrorReportType type)
        {
            ErrorType = type;
        }

        public void SetProblemType(ErrorReportProblemType type)
        {
            problemType = type;
        }
    }
}
