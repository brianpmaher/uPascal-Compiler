BR L0
L1:
ADD SP #1 SP
PUSH #2
POP 2(D1)
PUSH #"If procedures are working, this will print"
WRTS
PUSH #""
WRTLNS
SUB SP #1 SP
RET
L0:
PUSH D0
ADD SP #1 SP
SUB SP #2 D0
PUSH #"This is test 1 of the A level"
WRTS
PUSH #""
WRTLNS
PUSH #"This will test scope and procedure calling"
WRTS
PUSH #""
WRTLNS
PUSH #1
POP 1(D0)
PUSH D1
SUB SP #1 D1
CALL L1
SUB SP #0 SP
POP D1
PUSH #"\na is "
WRTS
PUSH 1(D0)
WRTS
PUSH #""
WRTLNS
PUSH #"If a is 1, scope is kept. If a is 2, variable scope is implemented incorrectly"
WRTS
PUSH #""
WRTLNS
SUB SP #1 SP
POP D0
HLT
