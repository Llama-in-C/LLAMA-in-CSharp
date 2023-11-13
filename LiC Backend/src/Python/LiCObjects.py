import json

from enum import Enum
from dataclasses import dataclass
from typing import Optional


class CallType(Enum):
    Initialize = 0
    Generate = 1
    SwapModel = 2

    @classmethod
    def from_value(cls, value):
        # Find the enum member with the matching value
        for member in cls:
            if member.value == value:
                return member
        # If no matching value is found, raise an error
        raise ValueError(f"No matching enum for value: {value}")


@dataclass
class PipePayload:
    CallType: CallType
    InputText: Optional[str]
    PathToModel: Optional[str]

    @staticmethod
    def from_json(json_data):
        parsed_json = json.loads(json_data)
        parsed_json['CallType'] = CallType.from_value(parsed_json['CallType'])
        return PipePayload(**parsed_json)


@dataclass
class PipeResponse:
    Error: Optional[str]
    Code: int
    Output: Optional[str]
    TimeTotal: Optional[float]
    MaxNewTokens: Optional[int]
