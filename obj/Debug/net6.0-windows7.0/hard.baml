   M S B A M L   `   `   `  ����D  @SnakeGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:SnakeGame	SnakeGame  5   	       SnakeGame.hard   5	   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5
   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:SnakeGame     5   	   -   5          � Title$  Snake��5   	   $	��500��6   $	��800��6$   $	��300��5   	   $	��300��6      BackgroundColor#��[  5   	     	TextColor#��[ 5   	     MainFont#��[ 5   	    � WindowStartupLocation$ CenterScreen=�5   	    � Icon$ GameComponents/icon.ico��5   	   .��5      (� .��5   
   �  � RowDefinitions 5       �� 5!      $
��auto �6    �� 5"      $��* �6    .+�5%      �� -   5         ��Name$ 	ScoreText��5%      $��Center=�5&      $��Center=�5'      $��Очки: 0��5(      $��22(�5)      $��10q�5*      �� 5+      -   5         ��Name$ 
GridBorder��5+      $��1��5,        GridLineColor#��[ 5-      $	��1.5q�5.      $��Center=�5/      $��Center=�50       �� EdgeMode$ Aliased=�51      $��
20,0,20,20q�52      ._�53      A� -   5         A�Name$ GameGrid��53      $	��400��54      $	��400��55        GridBackgroundColor#��[ 56       J� SnapsToDevicePixels$
 True��57      	 �� BitmapScalingMode$	 HighQuality=�58      59      5:      r� 5;      -   5        
 r�Name$

 qwee��5;       r� UnloadedBehavior$ Manual=�6)   $
��Left=�6C   $	��100��6^   $��
299,19,0,0q�6k   $��1��6   $	��Top=�6�   $	��100��6�    r� Source$* $C:\original SnakeGame\sound\hard.mp3?�6�    r� IsMuted$
 True��6�   �� 5<      -   5        $ Overlay��5<      $��1��5=        OverlayColor#��[ 5>      $ Aliased=�5B      ��5?      ��  �� Path$ ActualWidth� �� ElementName$ 
GridBorder����5@      �� $ ActualHeight�$ 
GridBorder����5A      �� $ Margin�$ 
GridBorder��._�5C      �� -   5        $ OverlayText��5C      $,��&Нажмите любую кнопку��5D      $��26(�5E      $��Center=�5F      $��Center=�5G      $
��Wrap=�5H      5K      �� 5L      -   5        $��Меню��5L      $
��Left=�6$   $��	636,0,0,0q�6?   $��1��6R   $	��Top=�6_   $��32��6w   $��65��6�   6�   �� 5M      -	   5        $��
Выход��5M      $
��Left=�6%   $	��Top=�6@   $��37��6X   $��65��6d   $��1��6o   6|   � 5N      $��1��6   5P      5R      5S      