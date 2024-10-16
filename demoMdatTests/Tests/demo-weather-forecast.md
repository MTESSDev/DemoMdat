# DemoWeatherForecast

## Valider le résultat du service de météo

Test qui s'assure que les prévisions des 5 prochains jours sont reçues et les dates correspondent

``````yaml
# Résultat attendu
expected:

  expectedRetour:
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