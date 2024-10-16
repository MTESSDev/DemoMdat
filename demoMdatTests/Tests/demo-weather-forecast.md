# DemoWeatherForecast

## Valider le r�sultat du service de m�t�o

Test qui s'assure que les pr�visions des 5 prochains jours sont re�ues et les dates correspondent

``````yaml
# R�sultat attendu
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