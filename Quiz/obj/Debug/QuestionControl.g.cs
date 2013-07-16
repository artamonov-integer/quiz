﻿#pragma checksum "..\..\QuestionControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "016E7088B5090C15607C61E83033AF8E"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.17929
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Quiz {
    
    
    /// <summary>
    /// QuestionControl
    /// </summary>
    public partial class QuestionControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 18 "..\..\QuestionControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox NumTextBox;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\QuestionControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox QuestionTextBox;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\QuestionControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox FilterTextAnswer;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\QuestionControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox AnswerComboBox;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\QuestionControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddImageButton;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\QuestionControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image QuestionImage;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Quiz;component/questioncontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\QuestionControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.NumTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.QuestionTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.FilterTextAnswer = ((System.Windows.Controls.TextBox)(target));
            
            #line 21 "..\..\QuestionControl.xaml"
            this.FilterTextAnswer.KeyUp += new System.Windows.Input.KeyEventHandler(this.FilterTextAnswer_KeyUp);
            
            #line default
            #line hidden
            return;
            case 4:
            this.AnswerComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 22 "..\..\QuestionControl.xaml"
            this.AnswerComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.AnswerComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.AddImageButton = ((System.Windows.Controls.Button)(target));
            
            #line 25 "..\..\QuestionControl.xaml"
            this.AddImageButton.Click += new System.Windows.RoutedEventHandler(this.AddImageButton_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.QuestionImage = ((System.Windows.Controls.Image)(target));
            
            #line 26 "..\..\QuestionControl.xaml"
            this.QuestionImage.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.QuestionImageClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

