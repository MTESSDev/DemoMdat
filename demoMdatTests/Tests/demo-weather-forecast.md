# DemoWeatherForecast

## Case 1

Description

``````yaml
# Résultat attendu
expected:
  expectedRetour:
    name: null
    generateExpectedData: null
    verify: 
      - type: match
        jsonPath: $
        allowAdditionalProperties: true
        data: 
          StatusCode: 200
          Content: 
            - Date: 2024-10-16
            - Date: 2024-10-17
            - Date: 2024-10-18
            - Date: 2024-10-19
            - Date: 2024-10-20
``````