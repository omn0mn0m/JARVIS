using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Word = Microsoft.Office.Interop.Word;

namespace JARVIS.Util
{
    class OfficeManager
    {
        private bool hasExcel;
        private bool hasPowerPoint;
        private bool hasWord;

        private Excel.Application excelApplication;
        private PowerPoint.Application powerPointApplication;
        private Word.Application wordApplication;

        private PowerPoint.Presentation presentation;
        private PowerPoint.Slides slides;
        private PowerPoint.Slide slide;
        private int slideCount;
        
        public OfficeManager()
        {

        }

        public enum ApplicationType
        {
            Excel, PowerPoint, Word
        }

        public void CheckForApplication(ApplicationType application)
        {
            try
            {
                switch (application)
                {
                    case ApplicationType.Excel:
                        excelApplication = (Excel.Application) Marshal.GetActiveObject("Excel.Application");
                        hasPowerPoint = true;
                        break;
                    case ApplicationType.PowerPoint:
                        powerPointApplication = (PowerPoint.Application) Marshal.GetActiveObject("PowerPoint.Application");
                        hasPowerPoint = true;
                        break;
                    case ApplicationType.Word:
                        wordApplication = (Word.Application) Marshal.GetActiveObject("Word.Application");
                        hasPowerPoint = true;
                        break;
                    default:
                        break;
                } 
            } catch {}

            switch (application)
            {
                case ApplicationType.Excel:
                    if (excelApplication != null)
                    {

                    }
                    break;
                case ApplicationType.PowerPoint:
                    if (powerPointApplication != null)
                    {
                        presentation = powerPointApplication.ActivePresentation;
                        slides = presentation.Slides;
                        slideCount = slides.Count;

                        try
                        {
                            slide = slides[powerPointApplication.ActiveWindow.Selection.SlideRange.SlideNumber];
                        }
                        catch
                        {
                            slide = powerPointApplication.SlideShowWindows[1].View.Slide;
                        } 
                    }
                    break;
                case ApplicationType.Word:
                    if (wordApplication != null)
                    {

                    }
                    break;
                default:
                    break;
            }
        }

        public void goToFirstSlide()
        {
            if (hasPowerPoint)
            {
                try
                {
                    slides[1].Select();
                    slide = slides[1];
                }
                catch
                { 
                    powerPointApplication.SlideShowWindows[1].View.First();
                    slide = powerPointApplication.SlideShowWindows[1].View.Slide;
                } 
            }
        }

        public void goToLastSlide()
        {
            try
            {
                slides[slideCount].Select();
                slide = slides[slideCount];
            }
            catch
            {
                powerPointApplication.SlideShowWindows[1].View.Last();
                slide = powerPointApplication.SlideShowWindows[1].View.Slide;
            }
        }

        public void goToNextSlide()
        {
            int slideIndex = slide.SlideIndex + 1;
            if (slideIndex > slideCount)
            {
                // TODO Add something for if you're at the last slide
            }
            else
            {
                try
                {
                    slide = slides[slideIndex];
                    slides[slideIndex].Select();
                }
                catch
                {
                    powerPointApplication.SlideShowWindows[1].View.Next();
                    slide = powerPointApplication.SlideShowWindows[1].View.Slide;
                }
            } 
        }

        public void goToPreviousSlide()
        {
            int slideIndex = slide.SlideIndex - 1;
            if (slideIndex >= 1)
            {
                try
                {
                    slide = slides[slideIndex];
                    slides[slideIndex].Select();
                }
                catch
                {
                    powerPointApplication.SlideShowWindows[1].View.Previous();
                    slide = powerPointApplication.SlideShowWindows[1].View.Slide;
                }
            }
            else
            {
                // TODO Add something for when you are already at the first page
            }
        }
    }
}
