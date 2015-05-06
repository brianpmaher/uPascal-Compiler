BR L0
L0:
PUSH D0
ADD SP #3 SP
SUB SP #4 D0
PUSH #"This is test 7 of B level"
WRTS
PUSH #""
WRTLNS
PUSH #"Please enter a positive integer"
WRTS
PUSH #""
WRTLNS
RD 1(D0)
PUSH 1(D0)
POP 2(D0)
