   M S B A M L   `   `   `  ����B  >GUI_WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:GUI_WPFGUI_WPF  5   	       GUI_WPF.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:GUI_WPF     5   	     � Title$  Server Handler GUI��5   	   $	��630��6$   $	��758��61   .��5	      � $��
0,0,-58,-6q�6   .+�5
   
   �� $
��Left=�6   $��82��6+   $��	46,10,0,0q�67   $	��Top=�6J   $��75��6b   $��/computer-156951_640.png��6m    J� RenderTransformOrigin$ 2.013,-5.217&�6�   �� 5   
   $��Sign In��6   $
��Left=�6"   $��	576,0,0,0q�6=   $��18(�6P   $	��Top=�6^   �� 5   
   $
��Left=�6@   $��
556,34,0,0q�6[    �� TextWrapping$
 Wrap=�6o   $	��Top=�6�   $	��120��6�   $��22��6�   ��6   �� *AuthModel.Email+ �� Mode$ TwoWay=�6�   �� 5   
   $
��Left=�6C   $��
556,65,0,0q�6^   $
 Wrap=�6r   $	��Top=�6�   $	��120��6�   $��23��6�   ��6   �� *AuthModel.Password+$ TwoWay=�6�   �� 5   
   $
��Left=�6F   $��556,100,0,0q�6a   $
 Wrap=�6v   $	��Top=�6�   $	��120��6�   $��24��6�   ��6   �� *AuthModel.ChatChannel+$ TwoWay=�6�   �� 5   
   -   5         ��Name$ 	signInBtn��5   Q   $��Sign in��6d   $
��Left=�6v   $��556,136,0,0q�6�   $	��Top=�6�   $��55��6�   ��6   �� *-+AuthenticationEvents.SubmitLoginInfoCommand+6�   �� 5   
   $��Sign out��6R   $
��Left=�6e   $��616,136,0,0q�6�   $	��Top=�6�   $��60��6�   ��6   �� *.,AuthenticationEvents.SubmitLogoutInfoCommand+6�   �� 5   
   $��E-mail: ��6   $��13(�6#   $
��Left=�61   $��
442,30,0,0q�6L   $	��Top=�6`   $ -0.298,0.079&�6x   $��32��6�   $��76��6�   �� 5   
   $��
Password: ��6   $��13(�6%   $
��Left=�63   $��
442,62,0,0q�6N   $	��Top=�6b   $ -0.298,0.079&�6z   $��32��6�   $��92��6�   �� 5   
   $��Chat channel:��6   $��13(�6(   $
��Left=�66   $��
442,96,0,0q�6Q   $	��Top=�6e   $ -0.298,0.079&�6}   $��28��6�   $	��103��6�   �� 5   
   $��File Manager��6N   $
��Left=�6e   $��
44,177,0,0q�6�   $	��Top=�6�   ��6   �� **(FileManagerEvents.FileManagerMenuCommand+6�   �� 5   
   $
��Chat��6E   $
��Left=�6T   $��
44,204,0,0q�6o   $	��Top=�6�   $��73��6�   ��6   �� * ChatManagerEvents.OpenChatMenu+6�   �� 5   
   $��Server Handler GUI��6   $��20(�6-   $
��Left=�6;   $��
126,35,0,0q�6V   $	��Top=�6j   $	��185��6�   � 5   
   -   5        $��fileManagerGrid��5      $y�Hidden=�6&   $��147,177,36,25q�6:   .+�5      &   System.Windows.Controls.DataGrid     SelectionMode$ Single=�6[   $��
0,26,10,10q�6�   ��6   �� * FileManagerModel.TableContents+$ TwoWay=� �� SelectedItem 6s   �� *" FileManagerModel.SelectedElement+$ TwoWay=�6�   �� 5      $��Upload��6   $
��Left=�6^   $��0,-1,0,0q�6y   $	��Top=�6�   $��80��6�   ��6&   �� *%#FileManagerEvents.UploadFileCommand+6�   �� 5      $��Remove��6   $
��Left=�6^   $��	90,-1,0,0q�6y   $	��Top=�6�   $��80��6�   ��6&   �� *%#FileManagerEvents.RemoveFileCommand+6�   �� 5      $��Download��6   $
��Left=�6b   $��
179,-1,0,0q�6}   $	��Top=�6�   $��80��6�   ��6(   �� *'%FileManagerEvents.DownloadFileCommand+6�   5      � 5   
   -   5        $��chatGrid��5      $��147,177,36,25q�6   .+�5      �� $
��Send��6   $
��Left=�6^   $��435,339,0,0q�6y   $	��Top=�6�   $��72��6�   $��27��6�   ��6$   �� *'%ChatManagerEvents.SubmitClientMessage+6�   �� 5      $
��Left=�6N   $	��Top=�6i   $	��505��6�   $	��279��6�   $��2,0,0,0q�6�   ��6   �� *ChatModel.Messages+$ TwoWay=�6�   �� 5       $
��Left=�6M   $��	2,339,0,0q�6h   $
 Wrap=�6{   $	��Top=�6�   $	��411��6�   $��27��6�   ��6   �� *ChatModel.CurrentMessage+$ TwoWay=�6�   5!      5"      5#      