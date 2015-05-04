BR L0
L0:
PUSH D0
ADD SP #3 SP
SUB SP #4 D0
PUSH #"This is test 6 of C level"
WRTS
PUSH #""
WRTLNS
PUSH #"This is the same as 5, without the string"
WRTS
PUSH #""
WRTLNS
PUSH #""
WRTS
PUSH #""
WRTLNS
PUSH #"Enter in any integer"
WRTS
PUSH #""
WRTLNS
RD 2(D0)
PUSH #"You entered: "
WRTS
PUSH 2(D0)
WRTS
PUSH #""
WRTLNS
PUSH #"Enter in any float"
WRTS
PUSH #""
WRTLNS
RDF 3(D0)
PUSH #"You entered: "
WRTS
PUSH 3(D0)
WRTS
PUSH #""
WRTLNS
SUB SP #3 SP
POP D0
HLT
