L0:
PUSH D0
MOV SP D0
ADD SP #1 SP
PUSH #0
POP 0(D0)
PUSH 0(D0)
PUSH #1
CMPLTS
BRFS L1
PUSH #2
POP 0(D0)
BR L2
L1:
PUSH #3
POP 0(D0)
L2:
PUSH #4
POP 0(D0)
SUB SP #1 SP
POP D0
HLT