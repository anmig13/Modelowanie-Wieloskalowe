   M S B A M L   `   `   `  ����K  GGameOfLife_byAnn, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:GameOfLife_byAnnGameOfLife_byAnn  5   	   !    GameOfLife_byAnn.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:GameOfLife_byAnn     5   	     � Title$  
MainWindow��5   	   $	��800��6   $	��950��6)   .��5	      � .+�5
   
   �� -   5         ��Name$ label��5
      $��Label��6   $
��Left=�6/   $��	359,8,0,0q�6J   $	��Top=�6]   $��32��6u   $	��278��6�    J� RenderTransformOrigin$ 0.5,0.5&�6�   $��18(�6�   $
��Bold"�6�   
�������z�5      V� 5      .~�5      �� �� 5       �� AngleX$ -1.685Q�6$   �� 5       �� Angle$ -0.595Q�6&   T� 5       T� X$ -0.471Q�6)   5      5      �� 5   
   -   5         ��Name$ buttonStart��5      $
��Left=�6&   $��	40,84,0,0q�6A   $	��Top=�6T   $��98��6l   $ -0.708,0.554&�6w   $��33��6�   $��START��6�   �� 5   
   -   5        $ buttonStartStop��5      $��Start animation��6*   $
��Left=�6D   $��
40,136,0,0q�6_   $	��Top=�6s   $��98��6�   $��35��6�   6�   �� 5   
   -   5         ��Name$ textBox��5      $
��Left=�6#   $��22��6>   $��
88,220,0,0q�6J    �� TextWrapping$
 Wrap=�6^   $	��Top=�6r   $��98��6�   6�   	� 5   
   -   5        	 	�Name$	 stable_button��5      $��Stable��6-   $
��Left=�6>   $��
26,322,0,0q�6Y   $	��Top=�6m   $��18��6�   $��60��6�   6�   	� 5   
   -   5        $	 glider_button��5      $��Glider��6-   $
��Left=�6>   $��
26,355,0,0q�6Y   $	��Top=�6m   	� 5   
   -   5        $	 manual_button��5      $��Manual definition��6-   $
��Left=�6I   $��
26,391,0,0q�6d   $	��Top=�6x   	� 5   
   -   5        $	 oscilator_button��5      $��	Oscilator��60   $
��Left=�6D   $��
26,424,0,0q�6_   $	��Top=�6s   	� 5   
   -	   5        $	 random_button��5      $��Random��6-   $
��Left=�6>   $��
26,462,0,0q�6Y   $	��Top=�6m   $ 
0.588,-0.8&�6�   �� 5   
   -
   5        
 ��Name$
 board��5      $
��Left=�6    $	��700��6;   $��
209,49,0,0q�6H   $	��Top=�6\   $	��700��6t   �� 5   
   -   5        $ labelS��5      $��Select initial state: ��6    $
��Left=�6A   $��
26,284,0,0q�6\   $	��Top=�6p   �� 5   
   $��Number of live cells:��6   $
��Left=�60   $��
21,492,0,0q�6K   $	��Top=�6_   �� 5   
   -   5        $ textBox1��5      $
��Left=�6$   $��23��6?   $��
40,523,0,0q�6K   $
 Wrap=�6_   $	��Top=�6s   $��98��6�   6�   �� 5    
   $��Set size of the board:��6   $
��Left=�61   $��
21,189,0,0q�6L   $	��Top=�6`   $	��128��6x   �� 5!   
   $��height��6   $
��Left=�6!   $��
21,220,0,0q�6<   $	��Top=�6P   $��49��6h   �� 5"   
   $��width��6   $
��Left=�6    $��
21,251,0,0q�6;   $	��Top=�6O   $ -0.289,0.423&�6g   �� 5#   
   -   5        $ textBoxw��5#      $
��Left=�6$   $��23��6?   $��
88,256,0,0q�6K   $
 Wrap=�6_   $	��Top=�6s   $��98��6�   6�   �� 5$   
   -   5        $ buttonClearClear��5$      $��Clear��6+   $
��Left=�6;   $��	40,34,0,0q�6V   $	��Top=�6i   $��98��6�   $��31��6�   5&      5'      