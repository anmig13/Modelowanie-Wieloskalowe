   M S B A M L   `   `   `  ����L  HGrainGrowth_byAnn, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:GrainGrowth_byAnnGrainGrowth_byAnn  5   	   "    GrainGrowth_byAnn.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:GrainGrowth_byAnn     5   	     � Title$  
MainWindow��5   	   $	��950��6   $
��1050��6)   .��5	      � .+�5
   
   �� -   5         ��Name$ ButtonStart��5
      $��Start��6&   $
��Left=�66   $��	52,75,0,0q�6Q   $	��Top=�6d   $��87��6|   $��26��6�   6�   �� 5   
   -   5        $ Button_animation��5      $��Start animation��6+   $
��Left=�6E   $��
52,117,0,0q�6`   $	��Top=�6t   $��87��6�   $��26��6�   6�   �� 5   
   $��	Set size:��6   $
��Left=�6$   $��
53,153,0,0q�6?   $	��Top=�6S   $��58��6k   �� 5   
   -   5         ��Name$ textBox��5      $
��Left=�6#   $��23��6>   $��
53,179,0,0q�6J    �� TextWrapping$
 Wrap=�6^   $	��Top=�6r   $��87��6�   6�   �� 5   
   -   5         ��Name$ label2��5      $"��Type of boundary conditions:��6    $
��Left=�6G   $��
21,269,0,0q�6b   $	��Top=�6v   �� 5   
   -   5         ��Name$ checkBox_periodic��5      $��periodic��6.   $
��Left=�6A   $��
52,300,0,0q�6\   $	��Top=�6p   $��25��6�   $��76��6�   �� 5   
   $��Type of neighborhood:��6   $
��Left=�60   $��
21,339,0,0q�6K   $	��Top=�6_   �� 5   
   $��Nucleation:��6   $
��Left=�6&   $��
21,403,0,0q�6A   $	��Top=�6U   	� 5   
   -   5         	�Name$ radioButtonEvenly��5      $��evenly��61   $
��Left=�6B   $��
36,434,0,0q�6]   $	��Top=�6q   	� 5   
   -   5        $ radioButtonRandom��5      $��random��61   $
��Left=�6B   $��
36,574,0,0q�6]   $	��Top=�6q    J� RenderTransformOrigin$ 0.506,0.867&�6�   	� 5   
   -   5        $ radioButtonManual��5      $��manual��61   $
��Left=�6B   $��
36,635,0,0q�6]   $	��Top=�6q   	� 5   
   -	   5        $ radioButtonRandomR��5      $��random with R��62   $
��Left=�6J   $��
36,662,0,0q�6e   $	��Top=�6y   �� 5   
   $��Grain Growth��6   $
��Left=�6'   $��	449,0,0,0q�6B   $	��Top=�6U   $��18(�6m   $
��Bold"�6{   $ 1.153,0.032&�6�   �� 5   
   -
   5         ��Name$ board��5      $
��Left=�6    $	��800��6;   $��
199,39,0,0q�6H   $	��Top=�6\   $	��800��6t   �� 5   
   -   5        $ textBoxR��5      $
��Left=�6$   $��23��6?   $��
52,694,0,0q�6K   $
 Wrap=�6_   $	��Top=�6s   $��81��6�   �� 5   
   $��Number in row:��6   $
��Left=�6)   $��
36,458,0,0q�6D   $	��Top=�6X   �� 5   
   -   5        $ 
textBoxRow��5      $
��Left=�6&   $��23��6A   $��
52,484,0,0q�6M   $
 Wrap=�6a   $	��Top=�6u   $��81��6�   �� 5   
   $��Number in column:��6   $
��Left=�6,   $��
36,506,0,0q�6G   $	��Top=�6[   �� 5   
   -   5        $ textBoxColumn��5      $
��Left=�6)   $��23��6D   $��
52,532,0,0q�6P   $
 Wrap=�6d   $	��Top=�6x   $��81��6�   �� 5   
   -   5        $ textBoxRandom��5      $
��Left=�6)   $��23��6D   $��
52,594,0,0q�6P   $
 Wrap=�6d   $	��Top=�6x   $��81��6�   6�   �� 5   
   -   5        $ checkBoxNeum��5      $��
von Neuman��6)   $
��Left=�6>   $��
52,370,0,0q�6Y   $	��Top=�6m   �� 5   
   $��R:��6   $
��Left=�6   $��
36,694,0,0q�68   $	��Top=�6L   $��23��6d   $��33��6p   �� 5    
   $��necleation number:��6   $
��Left=�6-   $��
36,726,0,0q�6H   $	��Top=�6\   $	��116��6t   �� 5!   
   -   5        $ textBoxLcell��5!      $
��Left=�6(   $��23��6C   $��
52,757,0,0q�6O   $
 Wrap=�6c   $	��Top=�6w   $��81��6�   $ 0.466,2.696&�6�   �� 5"   
   -   5        $ label5��5"      $��Label��6    $
��Left=�60   $��
10,795,0,0q�6K   $	��Top=�6_   $	��165��6w   $��53��6�   �� 5#   
   -   5        $ label6��5#      $��Label��6    $
��Left=�60   $��
21,853,0,0q�6K   $	��Top=�6_   $ -0.474,-0.846&�6w   �� 5$   
   -   5        $ buttonClearClear��5$      $��Clear��6+   $
��Left=�6;   $��	52,39,0,0q�6V   $	��Top=�6i   $��88��6�   $��25��6�   6�   �� 5%   
   -   5        $ textBoxheight��5%      $
��Left=�6)   $��23��6D   $��
53,223,0,0q�6P   $
 Wrap=�6d   $	��Top=�6x   $��87��6�   5'      5(      