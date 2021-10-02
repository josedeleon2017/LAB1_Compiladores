# DOC

## Tokens

T_ADD	+
T_SUB	-
T_MUL	*
T_DIV	/
T_LPAREN	(
T_RPAREN	)
T_INT	(0|1|2|3|4|5|6|7|8|9)^+
T_UNKNOWN	Todo lo demas
![image](https://user-images.githubusercontent.com/33106612/135702113-fcc2954b-0ac3-45e8-9439-9084f52ad445.png)


## Gramática final

S' → S

S → S+T | S - T | T

T → T / F | T * F | F

F → (S) | int | -int
