# DOC

## Tokens
T_ADD &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; +

T_SUB	&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; -

T_MUL &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; *

T_DIV &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; /

T_LPAREN &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; (

T_RPAREN &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; )

T_INT &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; (0|1|2|3|4|5|6|7|8|9)^+

T_UNKNOWN	&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Todo lo demas


![image](https://user-images.githubusercontent.com/33106612/135702125-cfed2213-93bf-4ed2-b905-46a572e62a43.png)
<br>

## Gramática final

S' → S

S → S+T | S - T | T

T → T / F | T * F | F

F → (S) | int | -int

## TABLA SLR(1)

![image](https://user-images.githubusercontent.com/33106612/135702278-8234c390-d64e-4370-9536-1e654babb218.png)

### Generación del ejemplo del enunciado

![image](https://user-images.githubusercontent.com/33106612/135702313-5811bb72-a612-4821-9309-458260ebbde7.png)

