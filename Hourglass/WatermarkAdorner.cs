﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace Hourglass
{
    public class WatermarkAdorner : Adorner
    {
        private readonly ContentPresenter contentPresenter;

        public WatermarkAdorner(UIElement adornedElement, object watermark)
            : base(adornedElement)
        {
            this.contentPresenter = new ContentPresenter();
            this.contentPresenter.Content = watermark;
            this.contentPresenter.HorizontalAlignment = HorizontalAlignment.Center;
            this.IsHitTestVisible = false;

            Binding opacityBinding = new Binding();
            opacityBinding.Source = adornedElement;
            opacityBinding.Path = new PropertyPath("Opacity");
            opacityBinding.Converter = new MultiplierConverter(0.5);
            BindingOperations.SetBinding(this, Adorner.OpacityProperty, opacityBinding);

            Binding visibilityBinding = new Binding();
            visibilityBinding.Source = adornedElement;
            visibilityBinding.Path = new PropertyPath("Visibility");
            BindingOperations.SetBinding(this, Adorner.VisibilityProperty, visibilityBinding);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            contentPresenter.Arrange(new Rect(finalSize));

            TextBox textBox = AdornedElement as TextBox;
            if (textBox != null)
            {
                TextElement.SetFontFamily(contentPresenter, textBox.FontFamily);
                TextElement.SetFontSize(contentPresenter, textBox.FontSize);
            }

            ComboBox comboBox = AdornedElement as ComboBox;
            if (comboBox != null)
            {
                TextElement.SetFontFamily(contentPresenter, comboBox.FontFamily);
                TextElement.SetFontSize(contentPresenter, comboBox.FontSize);
            }

            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return contentPresenter;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            contentPresenter.Measure(((Control)AdornedElement).RenderSize);
            return ((Control)AdornedElement).RenderSize;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }
    }
}