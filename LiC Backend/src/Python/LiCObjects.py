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
    InputText: Optional[str] = None
    PathToModel: Optional[str] = None
    Temperature: Optional[float] = None
    TopK: Optional[float] = None
    TopP: Optional[float] = None
    RepetitionPenalty: Optional[float] = None
    MaxNewTokens: Optional[int] = None

    @staticmethod
    def from_json(json_data):
        parsed_json = json.loads(json_data)
        parsed_json['CallType'] = CallType.from_value(parsed_json['CallType'])
        return PipePayload(**parsed_json)


@dataclass
class PipeResponse:
    Code: int
    Error: Optional[str] = None
    Output: Optional[str] = None
    TimeTotal: Optional[float] = None
    MaxNewTokens: Optional[int] = None
