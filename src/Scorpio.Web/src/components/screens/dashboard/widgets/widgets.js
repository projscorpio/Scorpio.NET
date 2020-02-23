import StatisticWidget from "./statisticWidget";
import GamepadAnalogs from "./gamepadAnalogs";
import ChartWidget from "./chartWidget";
import BatteryWidget from "./batteryWidget";
import UbiquitiWidget from "./ubiquitiWidget";
import MapWidget from "./mapWidget";

export const widgets = [
  {
    component: GamepadAnalogs,
    type: "GamepadAnalogs",
    dropdown: {
      key: "GamepadAnalogs",
      value: "GamepadAnalogs",
      text: "Gamepad analog previewer",
      description: "View pad state"
    }
  },
  {
    component: ChartWidget,
    type: "Chart",
    dropdown: {
      key: "Chart",
      value: "Chart",
      text: "Line chart",
      description: "Plot a chart"
    }
  },
  {
    component: StatisticWidget,
    type: "StatisticWidget",
    dropdown: {
      key: "StatisticWidget",
      value: "StatisticWidget",
      text: "Single value",
      description: "Display last seen value"
    }
  },
  {
    component: BatteryWidget,
    type: "BatteryWidget",
    dropdown: {
      key: "BatteryWidget",
      value: "BatteryWidget",
      text: "Battery",
      description: "Display battery value"
    }
  },
  {
    component: UbiquitiWidget,
    type: "UbiquitiWidget",
    dropdown: {
      key: "UbiquitiWidget",
      value: "UbiquitiWidget",
      text: "Ubiquiti",
      description: "Display ubiquiti stats"
    }
  },
  {
    component: MapWidget,
    type: "MapWidget",
    dropdown: {
      key: "MapWidget",
      value: "MapWidget",
      text: "Map",
      description: "Display gps map"
    }
  }
];
