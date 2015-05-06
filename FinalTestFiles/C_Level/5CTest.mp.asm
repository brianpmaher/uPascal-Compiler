BR L0
L0:
PUSH D0
ADD SP #3 SP
SUB SP #4 D0
PUSH #"This is test 5 for the C level"
WRTS
PUSH #""
WRTLNS
PUSH #"This test will center on reading in variables"
WRTS
PUSH #""
WRTLNS
PUSH #"This test might fail on strings"
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
PUSH #"Enter in any string"
WRTS
PUSH #""
WRTLNS
RDS 1(D0)
PUSH #"You entered: "
WRTS
PUSH 1(D0)
WRTS
PUSH #""
WRTLNS
SUB SP #3 SP
POP D0
HLT
