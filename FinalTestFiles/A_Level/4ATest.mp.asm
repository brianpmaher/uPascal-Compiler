BR L0
L2:
ADD SP #1 SP
PUSH #15.0
PUSH 1(D2)
CASTSF
MULSF
POP 1(D0)
PUSH #10
POP 1(D2)
PUSH #"This should put 45 into variable a"
WRTS
PUSH #""
WRTLNS
SUB SP #1 SP
RET
L1:
ADD SP #0 SP
PUSH #3
POP @1(D1)
PUSH D2
PUSH @1(D1)
SUB SP #2 D2
CALL L2
SUB SP #1 SP
POP D2
PUSH #"x in the function is 3"
POP -1(D1)
SUB SP #0 SP
RET
L0:
PUSH D0
ADD SP #19 SP
SUB SP #20 D0
PUSH #"This is test 5 of A level"
WRTS
PUSH #""
WRTLNS
PUSH #"This will test pass by value and pass by reference"
WRTS
PUSH #""
WRTLNS
PUSH #1.0
POP 1(D0)
PUSH #1
POP 19(D0)
ADD SP #1 SP
PUSH D1
PUSH D0
PUSH #19
ADDS
SUB SP #2 D1
CALL L1
SUB SP #1 SP
POP D1
POP 15(D0)
PUSH #"The reference variable is "
WRTS
PUSH 19(D0)
WRTS
PUSH #""
WRTLNS
PUSH #"If the reference variable is not 3, pass by reference is done incorrectly"
WRTS
PUSH #""
WRTLNS
PUSH #"a is "
WRTS
PUSH 1(D0)
WRTS
PUSH #". a should be 45"
WRTS
PUSH #""
WRTLNS
SUB SP #19 SP
POP D0
HLT
